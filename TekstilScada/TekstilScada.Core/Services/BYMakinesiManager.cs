// Services/BYMakinesiManager.cs
using HslCommunication;
using HslCommunication.Profinet.LSIS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekstilScada.Models; // BU SATIR EKLENDİ

namespace TekstilScada.Services
{
    // BatchSummaryData ve ChemicalConsumptionData sınıfları buradan SİLİNDİ.

    public class BYMakinesiManager : IPlcManager
    {
        private readonly LSFastEnet _plcClient;
        public string IpAddress { get; private set; }
        #region PLC Adres Sabitleri
        private const string ADIM_NO = "D3568";
        private const string RECETE_MODU = "KX30D";
        private const string PAUSE_DURUMU = "MX1015";
        private const string ALARM_NO = "D3604";
        private const string ANLIK_SU_SEVIYESI = "K200";
        private const string ANLIK_DEVIR = "D6007";
        private const string ANLIK_SICAKLIK = "D4980";
        private const string PROSES_YUZDESI = "D7752";
        private const string MAKINE_TIPI = "D6100";
        private const string SIPARIS_NO = "D6110";
        private const string MUSTERI_NO = "D6120";
        private const string BATCH_NO = "D6130";
        private const string OPERATOR_ISMI = "D6460";
        private const string RECETE_ADI = "D2550";
        private const string SU_MIKTARI = "D7702";
        private const string ELEKTRIK_HARCAMA = "D7720";
        private const string BUHAR_HARCAMA = "D7744";
        private const string CALISMA_SURESI = "D7750";
        private const string AKTIF_CALISMA = "MX2501";
        private const string TOPLAM_DURUS_SURESI_SN = "D7764";
        private const string STANDART_CEVRIM_SURESI_DK = "D6411";
        private const string TOPLAM_URETIM_ADEDI = "D7768";
        private const string HATALI_URETIM_ADEDI = "D7770";
        private const string ActualQuantity = "D7790";







        #endregion
        public BYMakinesiManager(string ipAddress, int port)
        {
            _plcClient = new LSFastEnet(ipAddress, port);
            this.IpAddress = ipAddress;
            _plcClient.ReceiveTimeOut = 5000;
        }

        // ... Bu dosyanın geri kalan tüm metotları aynı kalacak ...
        // (Connect, Disconnect, ReadLiveStatusData, WriteRecipeToPlcAsync vb.)

        public OperateResult Connect()
        {
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {IpAddress} (BY) -> Bağlantı deneniyor...");
            var result = _plcClient.ConnectServer();
            if (result.IsSuccess)
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {IpAddress} (BY) -> Bağlantı BAŞARILI.");
            else
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {IpAddress} (BY) -> Bağlantı BAŞARISIZ: {result.Message}");
            return result;
        }

        public OperateResult Disconnect()
        {
            return _plcClient.ConnectClose();
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
        public OperateResult<FullMachineStatus> ReadLiveStatusData()
        {
            var errorMessages = new List<string>();
            try
            {
                var status = new FullMachineStatus();
                bool anyReadFailed = false;
                var adimNoResult = _plcClient.ReadInt16(ADIM_NO);
                if (adimNoResult.IsSuccess) status.AktifAdimNo = adimNoResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {ADIM_NO} (Adım No) okunamadı: {adimNoResult.Message}"); anyReadFailed = true; }


                var receteModuResult = _plcClient.ReadBool(RECETE_MODU);
                if (!receteModuResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(receteModuResult);
                status.IsInRecipeMode = receteModuResult.Content;

              
                var pauseResult = _plcClient.ReadBool(PAUSE_DURUMU);
                if (pauseResult.IsSuccess) status.IsPaused = pauseResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {PAUSE_DURUMU} (Pause Durumu) okunamadı: {pauseResult.Message}"); anyReadFailed = true; }

                var alarmNoResult = _plcClient.ReadInt16(ALARM_NO);
                if (alarmNoResult.IsSuccess) { status.ActiveAlarmNumber = alarmNoResult.Content; status.HasActiveAlarm = alarmNoResult.Content > 0; }
                else { Debug.WriteLine($"[HATA] {IpAddress} - {ALARM_NO} (Alarm No) okunamadı: {alarmNoResult.Message}"); anyReadFailed = true; }

                var suSeviyesiResult = _plcClient.ReadInt16(ANLIK_SU_SEVIYESI);
                if (suSeviyesiResult.IsSuccess) status.AnlikSuSeviyesi = suSeviyesiResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {ANLIK_SU_SEVIYESI} (Anlık Su Seviyesi) okunamadı: {suSeviyesiResult.Message}"); anyReadFailed = true; }

                var devirResult = _plcClient.ReadInt16(ANLIK_DEVIR);
                if (devirResult.IsSuccess) status.AnlikDevirRpm = devirResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {ANLIK_DEVIR} (Anlık Devir) okunamadı: {devirResult.Message}"); anyReadFailed = true; }

                var sicaklikResult = _plcClient.ReadInt16(ANLIK_SICAKLIK);
                if (sicaklikResult.IsSuccess) status.AnlikSicaklik = sicaklikResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {ANLIK_SICAKLIK} (Anlık Sıcaklık) okunamadı: {sicaklikResult.Message}"); anyReadFailed = true; }

                var yuzdeResult = _plcClient.ReadInt16(PROSES_YUZDESI);
                if (yuzdeResult.IsSuccess) status.ProsesYuzdesi = yuzdeResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {PROSES_YUZDESI} (Proses Yüzdesi) okunamadı: {yuzdeResult.Message}"); anyReadFailed = true; }

                var operatorResult = ReadStringFromWords(OPERATOR_ISMI, 5);
                if (operatorResult.IsSuccess) status.OperatorIsmi = operatorResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {OPERATOR_ISMI} (Operatör İsmi) okunamadı: {operatorResult.Message}"); anyReadFailed = true; }

                var recipeNameResult = ReadStringFromWords(RECETE_ADI, 5);
                if (recipeNameResult.IsSuccess) status.RecipeName = recipeNameResult.Content;
                else { Debug.WriteLine($"[HATA] {IpAddress} - {RECETE_ADI} (Reçete Adı) okunamadı: {recipeNameResult.Message}"); anyReadFailed = true; }


                var suResult = _plcClient.ReadInt16(SU_MIKTARI);
                if (!suResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(suResult);
                status.SuMiktari = suResult.Content;

                var elektrikResult = _plcClient.ReadInt16(ELEKTRIK_HARCAMA);
                if (!elektrikResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(elektrikResult);
                status.ElektrikHarcama = elektrikResult.Content;

                var buharResult = _plcClient.ReadInt16(BUHAR_HARCAMA);
                if (!buharResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(buharResult);
                status.BuharHarcama = buharResult.Content;

                // YENİ: Çalışma Süresini Oku
                var runTimeResult = _plcClient.ReadInt16(CALISMA_SURESI);
                if (!runTimeResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(runTimeResult);
                status.CalismaSuresiDakika = runTimeResult.Content;

                // --- YENİ: OEE VERİLERİNİ OKUMA ---
                var isProductionResult = _plcClient.ReadBool(AKTIF_CALISMA);
                if (!isProductionResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(isProductionResult);
                status.IsMachineInProduction = isProductionResult.Content;

                // Toplam duruş süresini oku (32-bit)
                var downTimeResult = _plcClient.ReadInt32(TOPLAM_DURUS_SURESI_SN);
                if (!downTimeResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(downTimeResult);
                // Bu değeri kullanacağınız modeldeki ilgili alana atayın. Örnek:
                status.TotalDownTimeSeconds = downTimeResult.Content;

                // Standart çevrim süresini oku (16-bit)
                var cycleTimeResult = _plcClient.ReadInt16(STANDART_CEVRIM_SURESI_DK);
                if (!cycleTimeResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(cycleTimeResult);
                // Bu değeri kullanacağınız modeldeki ilgili alana atayın. Örnek:
                status.StandardCycleTimeMinutes = cycleTimeResult.Content;

                // Toplam üretim adedini oku (16-bit)
                var totalProdResult = _plcClient.ReadInt16(TOPLAM_URETIM_ADEDI);
                if (!totalProdResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(totalProdResult);
                // Bu değeri kullanacağınız modeldeki ilgili alana atayın. Örnek:
                 status.TotalProductionCount = totalProdResult.Content;

                // Hatalı üretim adedini oku (16-bit)
                var defectiveProdResult = _plcClient.ReadInt16(HATALI_URETIM_ADEDI);
                if (!defectiveProdResult.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(defectiveProdResult);
                // Bu değeri kullanacağınız modeldeki ilgili alana atayın. Örnek:
                 status.DefectiveProductionCount = defectiveProdResult.Content;
                // Hatalı üretim adedini oku (16-bit)
                var readActualQuantity = _plcClient.ReadInt16(ActualQuantity);
                if (!readActualQuantity.IsSuccess) return OperateResult.CreateFailedResult<FullMachineStatus>(readActualQuantity);
                status.ActualQuantityProduction = readActualQuantity.Content;


                if (errorMessages.Any())
                {
                    string combinedErrors = string.Join("\n", errorMessages);
                    Debug.WriteLine($"[PLC OKUMA HATASI] {IpAddress}:\n{combinedErrors}");
                    return new OperateResult<FullMachineStatus>($"PLC'den okuma hatası: {combinedErrors}");
                }
                status.ConnectionState = ConnectionStatus.Connected;
                return OperateResult.CreateSuccessResult(status);

                
            }
            catch (Exception ex)
            {
                return new OperateResult<FullMachineStatus>($"Okuma sırasında istisna oluştu: {ex.Message}");
            }
        }
        public Task<OperateResult> AcknowledgeAlarm()
        {
            // TODO: BYMakinesi için alarm onaylama bitini (örn: M100) true yapacak kodu yaz.
            // Şimdilik NotImplementedException fırlatıyoruz.
            throw new NotImplementedException("BYMakinesi için alarm onaylama henüz implemente edilmedi.");
        }
        public async Task<OperateResult> WriteRecipeToPlcAsync(ScadaRecipe recipe, int? recipeSlot = null)
        {
            if (recipe.Steps.Count != 98) return new OperateResult("Reçete 98 adım olmalıdır.");

            short[] fullRecipeData = new short[2450];
            foreach (var step in recipe.Steps)
            {
                int offset = (step.StepNumber - 1) * 25;
                if (offset + step.StepDataWords.Length <= fullRecipeData.Length)
                {
                    Array.Copy(step.StepDataWords, 0, fullRecipeData, offset, step.StepDataWords.Length);
                }
            }

            ushort chunkSize = 100;
            for (int i = 0; i < fullRecipeData.Length; i += chunkSize)
            {
                string currentAddress = $"D{100 + i}";
                short[] chunk = fullRecipeData.Skip(i).Take(chunkSize).ToArray();
                var writeResult = await Task.Run(() => _plcClient.Write(currentAddress, chunk));
                if (!writeResult.IsSuccess)
                {
                    return new OperateResult($"Reçete yazma hatası. Adres: {currentAddress}, Hata: {writeResult.Message}");
                }
            }

            byte[] recipeNameBytes = Encoding.ASCII.GetBytes(recipe.RecipeName.PadRight(10, ' ').Substring(0, 10));
            var nameWriteResult = await Task.Run(() => _plcClient.Write("D2550", recipeNameBytes));
            if (!nameWriteResult.IsSuccess)
            {
                return new OperateResult($"Reçete ismi yazma hatası: {nameWriteResult.Message}");
            }

            return OperateResult.CreateSuccessResult();
        }

        public async Task<OperateResult<short[]>> ReadRecipeFromPlcAsync()
        {
            short[] fullRecipeData = new short[2450];
            ushort chunkSize = 60;

            for (int i = 0; i < fullRecipeData.Length; i += chunkSize)
            {
                string currentAddress = $"D{100 + i}";
                ushort readLength = (ushort)Math.Min(chunkSize, fullRecipeData.Length - i);

                var readResult = await Task.Run(() => _plcClient.ReadInt16(currentAddress, readLength));
                if (!readResult.IsSuccess)
                {
                    return OperateResult.CreateFailedResult<short[]>(new OperateResult($"Reçete okunurken hata oluştu. Adres: {currentAddress}, Hata: {readResult.Message}"));
                }

                int lengthToCopy = Math.Min(readLength, readResult.Content.Length);
                Array.Copy(readResult.Content, 0, fullRecipeData, i, lengthToCopy);

                await Task.Delay(20);
            }

            return OperateResult.CreateSuccessResult(fullRecipeData);
        }

        public async Task<OperateResult<List<PlcOperator>>> ReadPlcOperatorsAsync()
        {
            var readResult = await Task.Run(() => _plcClient.ReadInt16("D7500", 60));
            if (!readResult.IsSuccess)
            {
                return OperateResult.CreateFailedResult<List<PlcOperator>>(readResult);
            }

            var operators = new List<PlcOperator>();
            var rawData = readResult.Content;

            for (int i = 0; i < 5; i++)
            {
                int offset = i * 12;
                byte[] nameBytes = new byte[20];
                Buffer.BlockCopy(rawData, offset * 2, nameBytes, 0, 20);
                string name = Encoding.ASCII.GetString(nameBytes).Trim('\0', ' ');

                operators.Add(new PlcOperator
                {
                    SlotIndex = i,
                    Name = name,
                    UserId = rawData[offset + 10],
                    Password = rawData[offset + 11]
                });
            }

            return OperateResult.CreateSuccessResult(operators);
        }

        public async Task<OperateResult> WritePlcOperatorAsync(PlcOperator plcOperator)
        {
            string startAddress = $"D{7500 + plcOperator.SlotIndex * 12}";
            byte[] dataToWrite = new byte[24];
            byte[] nameBytes = Encoding.ASCII.GetBytes(plcOperator.Name.PadRight(20).Substring(0, 20));
            Buffer.BlockCopy(nameBytes, 0, dataToWrite, 0, 20);
            BitConverter.GetBytes(plcOperator.UserId).CopyTo(dataToWrite, 20);
            BitConverter.GetBytes(plcOperator.Password).CopyTo(dataToWrite, 22);
            var writeResult = await Task.Run(() => _plcClient.Write(startAddress, dataToWrite));
            return writeResult;
        }

        public async Task<OperateResult<PlcOperator>> ReadSinglePlcOperatorAsync(int slotIndex)
        {
            string startAddress = $"D{7500 + slotIndex * 12}";

            var readResult = await Task.Run(() => _plcClient.ReadInt16(startAddress, 12));
            if (!readResult.IsSuccess)
            {
                return OperateResult.CreateFailedResult<PlcOperator>(readResult);
            }

            var rawData = readResult.Content;
            byte[] nameBytes = new byte[20];
            Buffer.BlockCopy(rawData, 0, nameBytes, 0, 20);
            string name = Encoding.ASCII.GetString(nameBytes).Trim('\0', ' ');

            var plcOperator = new PlcOperator
            {
                SlotIndex = slotIndex,
                Name = name,
                UserId = rawData[10],
                Password = rawData[11]
            };

            return OperateResult.CreateSuccessResult(plcOperator);
        }

        public async Task<OperateResult<BatchSummaryData>> ReadBatchSummaryDataAsync()
        {
            try
            {
                var summary = new BatchSummaryData();
                var waterResult = await Task.Run(() => _plcClient.ReadInt16("D7702"));
                if (!waterResult.IsSuccess) return OperateResult.CreateFailedResult<BatchSummaryData>(waterResult);
                summary.TotalWater = waterResult.Content;
                var electricityResult = await Task.Run(() => _plcClient.ReadInt16("D7720"));
                if (!electricityResult.IsSuccess) return OperateResult.CreateFailedResult<BatchSummaryData>(electricityResult);
                summary.TotalElectricity = electricityResult.Content;
                var steamResult = await Task.Run(() => _plcClient.ReadInt16("D7744"));
                if (!steamResult.IsSuccess) return OperateResult.CreateFailedResult<BatchSummaryData>(steamResult);
                summary.TotalSteam = steamResult.Content;
                return OperateResult.CreateSuccessResult(summary);
            }
            catch (Exception ex)
            {
                return new OperateResult<BatchSummaryData>($"Özet verileri okunurken istisna oluştu: {ex.Message}");
            }
        }

        public async Task<OperateResult<List<ChemicalConsumptionData>>> ReadChemicalConsumptionDataAsync()
        {
            var consumptionList = new List<ChemicalConsumptionData>();
            try
            {
                var namesResult = await Task.Run(() => _plcClient.ReadInt16("D6201", 90));
                if (!namesResult.IsSuccess) return OperateResult.CreateFailedResult<List<ChemicalConsumptionData>>(namesResult);

                var litersResult = await Task.Run(() => _plcClient.ReadInt16("D6351", 30));
                if (!litersResult.IsSuccess) return OperateResult.CreateFailedResult<List<ChemicalConsumptionData>>(litersResult);

                var stepsResult = await Task.Run(() => _plcClient.ReadInt16("D7250", 30));
                if (!stepsResult.IsSuccess) return OperateResult.CreateFailedResult<List<ChemicalConsumptionData>>(stepsResult);

                for (int i = 0; i < 30; i++)
                {
                    if (stepsResult.Content[i] > 0)
                    {
                        byte[] nameBytes = new byte[6];
                        Buffer.BlockCopy(namesResult.Content, i * 3 * 2, nameBytes, 0, 6);
                        string chemicalName = System.Text.Encoding.ASCII.GetString(nameBytes).Trim('\0', ' ');

                        consumptionList.Add(new ChemicalConsumptionData
                        {
                            StepNumber = stepsResult.Content[i],
                            ChemicalName = chemicalName,
                            AmountLiters = litersResult.Content[i]
                        });
                    }
                }
                return OperateResult.CreateSuccessResult(consumptionList);
            }
            catch (Exception ex)
            {
                return new OperateResult<List<ChemicalConsumptionData>>($"Kimyasal tüketim verileri okunurken hata: {ex.Message}");
            }
        }

        public async Task<OperateResult<List<ProductionStepDetail>>> ReadStepAnalysisDataAsync()
        {
            var stepDetails = new List<ProductionStepDetail>();
            try
            {
                var readResult = await Task.Run(() => _plcClient.ReadInt16("D6500", 392));
                if (!readResult.IsSuccess)
                {
                    return OperateResult.CreateFailedResult<List<ProductionStepDetail>>(readResult);
                }

                var rawData = readResult.Content;

                for (int i = 0; i < 98; i++)
                {
                    int offset = i * 4;
                    var step = new ProductionStepDetail
                    {
                        StepNumber = i + 1,
                        TheoreticalTime = rawData[offset].ToString(),
                        WorkingTime = rawData[offset + 1].ToString(),
                        StopTime = rawData[offset + 2].ToString(),
                        DeflectionTime = rawData[offset + 3].ToString()
                    };
                    stepDetails.Add(step);
                }

                return OperateResult.CreateSuccessResult(stepDetails);
            }
            catch (Exception ex)
            {
                return new OperateResult<List<ProductionStepDetail>>($"Adım analiz verileri okunurken hata: {ex.Message}");
            }
        }
        // YENİ: PLC'deki OEE sayaçlarını sıfırlayan metot
        public async Task<OperateResult> ResetOeeCountersAsync()
        {
            // D7764 (Duruş Süresi) adresine 0 yaz
            var downTimeResetResult = await Task.Run(() => _plcClient.Write("D7764", 0));
            if (!downTimeResetResult.IsSuccess)
            {
                return new OperateResult($"Duruş süresi sayacı sıfırlanamadı: {downTimeResetResult.Message}");
            }

            // D7770 (Hatalı Üretim) adresine 0 yaz
            var defectiveResetResult = await Task.Run(() => _plcClient.Write("D7770", 0));
            if (!defectiveResetResult.IsSuccess)
            {
                return new OperateResult($"Hatalı üretim sayacı sıfırlanamadı: {defectiveResetResult.Message}");
            }

            return OperateResult.CreateSuccessResult();
        }
        // YENİ: PLC'deki üretim sayacını bir artıran metot
        public async Task<OperateResult> IncrementProductionCounterAsync()
        {
            // D7768 adresindeki mevcut değeri oku
            var readResult = await Task.Run(() => _plcClient.ReadInt16("D7768"));
            if (!readResult.IsSuccess)
            {
                return new OperateResult($"Üretim sayacı okunamadı: {readResult.Message}");
            }

            // Değeri bir artır ve geri yaz
            short newCount = (short)(readResult.Content + 1);
            var writeResult = await Task.Run(() => _plcClient.Write("D7768", newCount));

            return writeResult;
        }
    }
}
