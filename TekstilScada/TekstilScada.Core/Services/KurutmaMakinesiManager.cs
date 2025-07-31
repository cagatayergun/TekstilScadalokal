// Services/KurutmaMakinesiManager.cs
using HslCommunication;
using HslCommunication.Profinet.LSIS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TekstilScada.Models;

namespace TekstilScada.Services
{
    public class KurutmaMakinesiManager : IPlcManager
    {
        private readonly LSFastEnet _plcClient;
        public string IpAddress { get; private set; }

        public KurutmaMakinesiManager(string ipAddress, int port)
        {
            _plcClient = new LSFastEnet(ipAddress, port);
            this.IpAddress = ipAddress;
            _plcClient.ReceiveTimeOut = 5000;
        }
        // YENİ: Eksik metot eklendi
        public Task<OperateResult> AcknowledgeAlarm()
        {
            throw new NotImplementedException("KurutmaMakinesi için alarm onaylama henüz implemente edilmedi.");
        }
        public async Task<OperateResult> WriteRecipeToPlcAsync(ScadaRecipe recipe, int? recipeSlot = null)
        {
            if (!recipeSlot.HasValue || recipeSlot < 1 || recipeSlot > 20)
                return new OperateResult("Geçersiz reçete numarası (1-20 arası olmalıdır).");

            if (recipe.Steps == null || recipe.Steps.Count == 0)
                return new OperateResult("Reçete adımı bulunamadı.");

            try
            {
                var isRunningResult = await Task.Run(() => _plcClient.ReadBool("K30C"));
                if (!isRunningResult.IsSuccess) return isRunningResult;
                if (isRunningResult.Content)
                {
                    return new OperateResult("Makine çalışırken reçete yüklenemez!");
                }

                var firstStep = recipe.Steps[0];
                short setTemperature = firstStep.StepDataWords[0];
                short setHumidity = firstStep.StepDataWords[1];
                short setDuration = firstStep.StepDataWords[2];
                short setRpm = firstStep.StepDataWords[3];
                short setCoolingTime = firstStep.StepDataWords[4];
                short controlWord = firstStep.StepDataWords[5];

                await Task.Run(() => _plcClient.Write("D3545", setTemperature));
                await Task.Run(() => _plcClient.Write("D3595", setHumidity));
                await Task.Run(() => _plcClient.Write("D3580", setDuration));
                await Task.Run(() => _plcClient.Write("D3680", setRpm));
                await Task.Run(() => _plcClient.Write("D3720", setCoolingTime));
                await Task.Run(() => _plcClient.Write("D3700", controlWord));

                await Task.Run(() => _plcClient.Write("D3610", (short)recipeSlot.Value));

                await Task.Run(() => _plcClient.Write("M1", true));
                await Task.Delay(500);
                await Task.Run(() => _plcClient.Write("M1", false));

                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return new OperateResult($"Reçete yazılırken hata: {ex.Message}");
            }
        }
        private OperateResult<string> ReadStringFromWords(string address, ushort wordLength)
        {
            // Veriyi önce ham word dizisi olarak oku
            var readResult = _plcClient.ReadInt16(address, wordLength);
            if (!readResult.IsSuccess)
            {
                // Hata durumunda, hangi adreste sorun olduğunu belirterek geri dön
                return OperateResult.CreateFailedResult<string>(new OperateResult($"Adres bloğu okunamadı: {address}, Hata: {readResult.Message}"));
            }

            try
            {
                // Okunan word dizisini byte dizisine çevir
                byte[] byteData = new byte[readResult.Content.Length * 2];
                Buffer.BlockCopy(readResult.Content, 0, byteData, 0, byteData.Length);

                // Byte dizisini ASCII metne çevir ve gereksiz karakterleri temizle
                string value = Encoding.ASCII.GetString(byteData).Trim('\0', ' ');
                return OperateResult.CreateSuccessResult(value);
            }
            catch (Exception ex)
            {
                return new OperateResult<string>($"String dönüşümü sırasında hata: {ex.Message}");
            }
        }
        public async Task<OperateResult<short[]>> ReadRecipeFromPlcAsync()
        {
            try
            {
                // GÜNCELLENDİ: Yeni okuma adresleri kullanılıyor.
                var tempResult = await Task.Run(() => _plcClient.ReadInt16("D3570"));
                if (!tempResult.IsSuccess) return OperateResult.CreateFailedResult<short[]>(tempResult);

                var humidityResult = await Task.Run(() => _plcClient.ReadInt16("D3600"));
                if (!humidityResult.IsSuccess) return OperateResult.CreateFailedResult<short[]>(humidityResult);

                var durationResult = await Task.Run(() => _plcClient.ReadInt16("D3615"));
                if (!durationResult.IsSuccess) return OperateResult.CreateFailedResult<short[]>(durationResult);

                var rpmResult = await Task.Run(() => _plcClient.ReadInt16("D7000"));
                if (!rpmResult.IsSuccess) return OperateResult.CreateFailedResult<short[]>(rpmResult);

                var coolingResult = await Task.Run(() => _plcClient.ReadInt16("D3724"));
                if (!coolingResult.IsSuccess) return OperateResult.CreateFailedResult<short[]>(coolingResult);

                // Okunan değerleri standart bir dizi formatında geri döndür
                short[] recipeData = new short[5];
                recipeData[0] = tempResult.Content;
                recipeData[1] = humidityResult.Content;
                recipeData[2] = durationResult.Content;
                recipeData[3] = rpmResult.Content;
                recipeData[4] = coolingResult.Content;

                // TODO: D3700.0 ve D3700.1 bitlerini okuyup 6. word olarak eklemek gerekebilir.
                // Şimdilik 5 word olarak bırakıyoruz.

                return OperateResult.CreateSuccessResult(recipeData);
            }
            catch (Exception ex)
            {
                return new OperateResult<short[]>($"Kurutma reçetesi okunurken hata: {ex.Message}");
            }
        }

        // ... Bu dosyadaki diğer tüm metotlar aynı kalabilir ...
        public OperateResult Connect()
        {
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {IpAddress} (Kurutma) -> Bağlantı deneniyor...");
            var result = _plcClient.ConnectServer();
            if (result.IsSuccess)
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {IpAddress} (Kurutma) -> Bağlantı BAŞARILI.");
            else
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {IpAddress} (Kurutma) -> Bağlantı BAŞARISIZ: {result.Message}");
            return result;
        }

        public OperateResult Disconnect()
        {
            return _plcClient.ConnectClose();
        }

        public OperateResult<FullMachineStatus> ReadLiveStatusData()
        {
            try
            {
                var status = new FullMachineStatus();
                var isRunningResult = _plcClient.ReadBool("M8000");
                if (!isRunningResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(isRunningResult);
                status.IsInRecipeMode = isRunningResult.Content;
                var tempResult = _plcClient.ReadInt16("D8000");
                if (!tempResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(tempResult);
                status.AnlikSicaklik = tempResult.Content;
                var fanRpmResult = _plcClient.ReadInt16("D8002");
                if (!fanRpmResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(fanRpmResult);
                status.AnlikDevirRpm = fanRpmResult.Content;
                var alarmNoResult = _plcClient.ReadInt16("D8010");
                if (!alarmNoResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(alarmNoResult);
                status.ActiveAlarmNumber = alarmNoResult.Content;
                status.HasActiveAlarm = alarmNoResult.Content > 0;
                var batchNoResult = _plcClient.ReadString("D8200", 20, Encoding.ASCII);
                if (!batchNoResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(batchNoResult);
                status.BatchNumarasi = batchNoResult.Content.Trim('\0', ' ');
                status.ConnectionState = ConnectionStatus.Connected;

                var isProductionResult = _plcClient.ReadBool("M8501");
                if (!isProductionResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(isProductionResult);
                status.IsMachineInProduction = isProductionResult.Content;

                var downTimeResult = _plcClient.ReadInt32("D8800");
                if (!downTimeResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(downTimeResult);
                status.TotalDownTimeSeconds = downTimeResult.Content;

                var totalProdResult = _plcClient.ReadInt16("D8802");
                if (!totalProdResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(totalProdResult);
                status.TotalProductionCount = totalProdResult.Content;

                var defectiveProdResult = _plcClient.ReadInt16("D8804");
                if (!defectiveProdResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(defectiveProdResult);
                status.DefectiveProductionCount = defectiveProdResult.Content;

                return OperateResult.CreateSuccessResult(status);
            }
            catch (Exception ex)
            {
                return new OperateResult<FullMachineStatus>($"Kurutma canlı veri okuma hatası: {ex.Message}");
            }
        }
        public async Task<OperateResult> ResetOeeCountersAsync()
        {
            await Task.Run(() => _plcClient.Write("D8800", 0)); // Duruş Süresi
            await Task.Run(() => _plcClient.Write("D8804", 0)); // Hatalı Üretim
            return OperateResult.CreateSuccessResult();
        }
        public async Task<OperateResult> IncrementProductionCounterAsync()
        {
            var readResult = await Task.Run(() => _plcClient.ReadInt16("D8802"));
            if (!readResult.IsSuccess) return new OperateResult($"Üretim sayacı okunamadı: {readResult.Message}");

            short newCount = (short)(readResult.Content + 1);
            return await Task.Run(() => _plcClient.Write("D8802", newCount));
        }
        public Task<OperateResult<List<PlcOperator>>> ReadPlcOperatorsAsync()
        {
            throw new NotImplementedException("Kurutma makineleri operatör yönetimini desteklemez.");
        }

        public Task<OperateResult> WritePlcOperatorAsync(PlcOperator plcOperator)
        {
            throw new NotImplementedException("Kurutma makineleri operatör yönetimini desteklemez.");
        }

        public Task<OperateResult<PlcOperator>> ReadSinglePlcOperatorAsync(int slotIndex)
        {
            throw new NotImplementedException("Kurutma makineleri operatör yönetimini desteklemez.");
        }

        public Task<OperateResult<BatchSummaryData>> ReadBatchSummaryDataAsync()
        {
            throw new NotImplementedException("Kurutma makinesi için özet veri okuma henüz yazılmadı.");
        }

        public Task<OperateResult<List<ChemicalConsumptionData>>> ReadChemicalConsumptionDataAsync()
        {
            return Task.FromResult(OperateResult.CreateSuccessResult(new List<ChemicalConsumptionData>()));
        }

        public Task<OperateResult<List<ProductionStepDetail>>> ReadStepAnalysisDataAsync()
        {
            return Task.FromResult(OperateResult.CreateSuccessResult(new List<ProductionStepDetail>()));
        }
       
    }
}
