// Dosya: TekstilScada.Core/Services/PlcPollingService.cs

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.Services
{
    public class PlcPollingService
    {
        // ... (Diğer alanlarınız aynı kalacak) ...
        public event Action<int, FullMachineStatus> OnMachineDataRefreshed;
        private ConcurrentDictionary<int, IPlcManager> _plcManagers;
        public ConcurrentDictionary<int, FullMachineStatus> MachineDataCache { get; private set; }
        private readonly AlarmRepository _alarmRepository;
        private readonly ProcessLogRepository _processLogRepository;
        private readonly ProductionRepository _productionRepository;
        private ConcurrentDictionary<int, string> _currentBatches;
        private ConcurrentDictionary<int, DateTime> _reconnectAttempts;
        private ConcurrentDictionary<int, ConnectionStatus> _connectionStates;
        private System.Threading.Timer _mainPollingTimer;
        private readonly int _pollingIntervalMs = 1000;
        private readonly int _loggingIntervalMs = 5000;
        private System.Threading.Timer _loggingTimer;
        public event Action<int, FullMachineStatus> OnMachineConnectionStateChanged;
        public event Action<int, FullMachineStatus> OnActiveAlarmStateChanged;
        private ConcurrentDictionary<int, AlarmDefinition> _alarmDefinitionsCache;
        private ConcurrentDictionary<int, short> _lastKnownStepNumbers;
        private ConcurrentDictionary<int, ConcurrentDictionary<int, DateTime>> _activeAlarmsTracker;


        // Constructor'ı (Yapıcı Metot) bu şekilde güncelleyin
        public PlcPollingService(AlarmRepository alarmRepository, ProcessLogRepository processLogRepository, ProductionRepository productionRepository)
        {
            _alarmRepository = alarmRepository;
            _processLogRepository = processLogRepository;
            _productionRepository = productionRepository;

            _plcManagers = new ConcurrentDictionary<int, IPlcManager>();
            MachineDataCache = new ConcurrentDictionary<int, FullMachineStatus>();
            _reconnectAttempts = new ConcurrentDictionary<int, DateTime>();
            _connectionStates = new ConcurrentDictionary<int, ConnectionStatus>();
            _activeAlarmsTracker = new ConcurrentDictionary<int, ConcurrentDictionary<int, DateTime>>();
            _currentBatches = new ConcurrentDictionary<int, string>();
            _lastKnownStepNumbers = new ConcurrentDictionary<int, short>();
        }

        // PollingTimer_Tick metodunu bu yeni ve daha akıllı versiyonla değiştirin
        private void PollingTimer_Tick(object state)
        {
            Parallel.ForEach(_plcManagers, kvp =>
            {
                int machineId = kvp.Key;
                IPlcManager manager = kvp.Value;

                // Her döngüde en güncel durumu önbellekten al
                var status = MachineDataCache[machineId];

                if (status.ConnectionState != ConnectionStatus.Connected)
                {
                    HandleReconnection(machineId, manager);
                }
                else
                {
                    var readResult = manager.ReadLiveStatusData();
                    if (readResult.IsSuccess)
                    {
                        // Başarılı okuma durumunda verileri güncelle
                        var newStatus = readResult.Content;
                        newStatus.MachineId = machineId;
                        newStatus.MachineName = status.MachineName; // İsim ve bağlantı durumunu koru
                        newStatus.ConnectionState = ConnectionStatus.Connected;

                        CheckAndLogBatch(machineId, newStatus);
                        CheckAndLogAlarms(machineId, newStatus);

                        status = newStatus; // status değişkenini yeni veriyle tamamen değiştir
                    }
                    else
                    {
                        // Başarısız okuma durumunda bağlantıyı kopar
                        HandleDisconnection(machineId);
                        status = MachineDataCache[machineId]; // Kapanmış durumu tekrar al
                    }
                }

                // ÖNEMLİ DEĞİŞİKLİK:
                // Her döngünün sonunda, makinenin durumu ne olursa olsun (bağlı, kopuk, alarmda vb.)
                // en güncel halini önbelleğe yaz ve olayı tetikle.
                MachineDataCache[machineId] = status;
                OnMachineDataRefreshed?.Invoke(machineId, status);
            });
        }

        // HandleDisconnection metodunu bu şekilde güncelleyin
        private void HandleDisconnection(int machineId)
        {
            var status = MachineDataCache[machineId];
            status.ConnectionState = ConnectionStatus.ConnectionLost;
            _connectionStates[machineId] = ConnectionStatus.ConnectionLost;
            _reconnectAttempts.TryAdd(machineId, DateTime.UtcNow);

            // Sadece bağlantı durumu değiştiğinde değil, genel veri yenileme olayını da tetikle
            OnMachineConnectionStateChanged?.Invoke(machineId, status);

            LiveEventAggregator.Instance.Publish(new LiveEvent
            {
                Source = status.MachineName,
                Message = "İletişim koptu!",
                Type = EventType.SystemWarning
            });
        }

        // Bu dosyadaki diğer tüm metotlarınız (Start, Stop, HandleReconnection, CheckAndLogBatch vb.) aynı kalabilir.
        // Sadece yukarıdaki iki metodu güncellediğinizden emin olun.

        // Diğer metotlarınızın tam listesi (değişiklik yok)
        public void Start(List<Machine> machines)
        {
            Stop();
            LoadAlarmDefinitionsCache();
            foreach (var machine in machines)
            {
                try
                {
                    var plcManager = PlcManagerFactory.Create(machine);
                    _plcManagers.TryAdd(machine.Id, plcManager);
                    _connectionStates.TryAdd(machine.Id, ConnectionStatus.Disconnected);
                    MachineDataCache.TryAdd(machine.Id, new FullMachineStatus { MachineId = machine.Id, MachineName = machine.MachineName, ConnectionState = ConnectionStatus.Disconnected });
                    _activeAlarmsTracker.TryAdd(machine.Id, new ConcurrentDictionary<int, DateTime>());
                    _currentBatches.TryAdd(machine.Id, null);
                    _lastKnownStepNumbers.TryAdd(machine.Id, 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex); // Hata yönetimi
                }
            }
            _mainPollingTimer = new System.Threading.Timer(PollingTimer_Tick, null, 500, _pollingIntervalMs);
            _loggingTimer = new System.Threading.Timer(LoggingTimer_Tick, null, 1000, _loggingIntervalMs);
        }

        public void Stop()
        {
            _mainPollingTimer?.Change(Timeout.Infinite, 0);
            _mainPollingTimer?.Dispose();
            _loggingTimer?.Change(Timeout.Infinite, 0);
            _loggingTimer?.Dispose();

            if (_plcManagers != null && !_plcManagers.IsEmpty)
            {
                Parallel.ForEach(_plcManagers.Values, manager => { manager.Disconnect(); });
            }
            _plcManagers.Clear();
            MachineDataCache.Clear();
            _connectionStates.Clear();
            _activeAlarmsTracker.Clear();
            _currentBatches.Clear();
            _lastKnownStepNumbers.Clear();
        }

        private void HandleReconnection(int machineId, IPlcManager manager)
        {
            if (!_reconnectAttempts.ContainsKey(machineId) || (DateTime.UtcNow - _reconnectAttempts[machineId]).TotalSeconds > 10)
            {
                _reconnectAttempts[machineId] = DateTime.UtcNow;
                var status = MachineDataCache[machineId];
                status.ConnectionState = ConnectionStatus.Connecting;
                _connectionStates[machineId] = ConnectionStatus.Connecting;
                OnMachineConnectionStateChanged?.Invoke(machineId, status);

                var connectResult = manager.Connect();
                if (connectResult.IsSuccess)
                {
                    status.ConnectionState = ConnectionStatus.Connected;
                    _connectionStates[machineId] = ConnectionStatus.Connected;
                    _reconnectAttempts.TryRemove(machineId, out _);
                    OnMachineConnectionStateChanged?.Invoke(machineId, status);

                    LiveEventAggregator.Instance.Publish(new LiveEvent
                    {
                        Timestamp = DateTime.Now,
                        Source = status.MachineName,
                        Message = "İletişim yeniden kuruldu.",
                        Type = EventType.SystemSuccess
                    });
                }
                else
                {
                    _connectionStates[machineId] = ConnectionStatus.Disconnected;
                }
            }
        }

        private void LoggingTimer_Tick(object state)
        {
            foreach (var machineStatus in MachineDataCache.Values)
            {
                // Koşulu düzelttik: Artık sadece bağlantının varlığını kontrol ediyoruz.
                if (machineStatus.ConnectionState == ConnectionStatus.Connected)
                {
                    try
                    {
                        // Parti varsa (üretimdeyse) normal üretim logu at.
                        // Bu çağrı kendi içinde parti kontrolü yapıyor (LogData metodu).
                        if (machineStatus.IsInRecipeMode)
                        {
                            _processLogRepository.LogData(machineStatus);
                        }
                        else // Parti yoksa (boştaysa) manuel log at.
                        {
                            // Artık sıcaklık veya devir koşulu yok. Makine boşta da olsa verisi kaydedilecek.
                            _processLogRepository.LogManualData(machineStatus);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Olası veritabanı hatalarını yakalamak için bu bloğu koruyoruz.
                        Console.WriteLine($"Makine {machineStatus.MachineId} için veri loglama hatası: {ex.Message}");
                    }
                }
            }
        }

        private void LoadAlarmDefinitionsCache()
        {
            try
            {
                var definitions = _alarmRepository.GetAllAlarmDefinitions();
                _alarmDefinitionsCache = new ConcurrentDictionary<int, AlarmDefinition>(
                    definitions.ToDictionary(def => def.AlarmNumber, def => def)
                );
            }
            catch (Exception)
            {
                _alarmDefinitionsCache = new ConcurrentDictionary<int, AlarmDefinition>();
            }
        }

        private void CheckAndLogBatch(int machineId, FullMachineStatus currentStatus)
        {
            _currentBatches.TryGetValue(machineId, out string lastBatchId);
            if (currentStatus.IsInRecipeMode && !string.IsNullOrEmpty(currentStatus.BatchNumarasi) && currentStatus.BatchNumarasi != lastBatchId)
            {
                _productionRepository.StartNewBatch(currentStatus);
                _currentBatches[machineId] = currentStatus.BatchNumarasi;
            }
            else if (!currentStatus.IsInRecipeMode && lastBatchId != null)
            {
                _productionRepository.EndBatch(machineId, lastBatchId, currentStatus);
                if (_plcManagers.TryGetValue(machineId, out var plcManager))
                {
                    Task.Run(async () => {
                        await plcManager.IncrementProductionCounterAsync();
                        await plcManager.ResetOeeCountersAsync();
                    });
                }
                _currentBatches[machineId] = null;
            }
        }

        private void CheckAndLogAlarms(int machineId, FullMachineStatus currentStatus)
        {
            if (_activeAlarmsTracker == null || !_activeAlarmsTracker.TryGetValue(machineId, out var machineActiveAlarms))
            {
                _activeAlarmsTracker?.TryAdd(machineId, new ConcurrentDictionary<int, DateTime>());
                return;
            }

            MachineDataCache.TryGetValue(machineId, out var previousStatus);
            int previousAlarmNumber = previousStatus?.ActiveAlarmNumber ?? 0;
            int currentAlarmNumber = currentStatus.ActiveAlarmNumber;

            // Yeni bir alarm geldiyse...
            if (currentAlarmNumber > 0)
            {
                // Ve bu alarm daha önce aktif değilse...
                if (!machineActiveAlarms.ContainsKey(currentAlarmNumber) && _alarmDefinitionsCache.TryGetValue(currentAlarmNumber, out var newAlarmDef))
                {
                    // Veritabanına 'ACTIVE' olarak kaydet ve olayı yayınla
                    _alarmRepository.WriteAlarmHistoryEvent(machineId, newAlarmDef.Id, "ACTIVE");
                    LiveEventAggregator.Instance.PublishAlarm(currentStatus.MachineName, newAlarmDef.AlarmText);
                }
                // Alarmı aktif listesine ekle veya başlangıç zamanını güncelle
                machineActiveAlarms[currentAlarmNumber] = DateTime.Now;
            }

            // Zaman aşımına uğrayan (artık PLC'den gelmeyen) alarmları temizle
            var timedOutAlarms = machineActiveAlarms.Where(kvp => (DateTime.Now - kvp.Value).TotalSeconds > 30).ToList();
            foreach (var timedOutAlarm in timedOutAlarms)
            {
                if (_alarmDefinitionsCache.TryGetValue(timedOutAlarm.Key, out var oldAlarmDef))
                {
                    _alarmRepository.WriteAlarmHistoryEvent(machineId, oldAlarmDef.Id, "INACTIVE");
                    LiveEventAggregator.Instance.Publish(new LiveEvent { Type = EventType.SystemInfo, Source = currentStatus.MachineName, Message = $"{oldAlarmDef.AlarmText} - TEMİZLENDİ" });
                }
                machineActiveAlarms.TryRemove(timedOutAlarm.Key, out _);
            }

            // PLC'den "alarm yok" (0) sinyali gelirse ve içeride hala aktif alarm varsa hepsini temizle
            if (currentAlarmNumber == 0 && !machineActiveAlarms.IsEmpty)
            {
                foreach (var activeAlarm in machineActiveAlarms)
                {
                    if (_alarmDefinitionsCache.TryGetValue(activeAlarm.Key, out var oldAlarmDef))
                    {
                        _alarmRepository.WriteAlarmHistoryEvent(machineId, oldAlarmDef.Id, "INACTIVE");
                    }
                }
                machineActiveAlarms.Clear();
            }

            // Anlık durum nesnesini en güncel bilgilere göre güncelle
            currentStatus.HasActiveAlarm = !machineActiveAlarms.IsEmpty;
            if (currentStatus.HasActiveAlarm)
            {
                // Birden fazla alarm aktifse, en son geleni göster
                currentStatus.ActiveAlarmNumber = machineActiveAlarms.OrderByDescending(kvp => kvp.Value).First().Key;
                if (_alarmDefinitionsCache.TryGetValue(currentStatus.ActiveAlarmNumber, out var def))
                {
                    currentStatus.ActiveAlarmText = def.AlarmText;
                }
                else
                {
                    currentStatus.ActiveAlarmText = $"TANIMSIZ ALARM ({currentStatus.ActiveAlarmNumber})";
                }
            }
            else
            {
                currentStatus.ActiveAlarmNumber = 0;
                currentStatus.ActiveAlarmText = "";
            }

            // Alarm durumunda bir değişiklik olduysa (yeni alarm geldi veya temizlendi), ilgili event'i tetikle
            if ((previousStatus?.HasActiveAlarm ?? false) != currentStatus.HasActiveAlarm || previousAlarmNumber != currentStatus.ActiveAlarmNumber)
            {
                OnActiveAlarmStateChanged?.Invoke(machineId, currentStatus);
            }
        }
        public List<AlarmDefinition> GetActiveAlarmsForMachine(int machineId)
        {
            var activeAlarms = new List<AlarmDefinition>();
            if (_activeAlarmsTracker.TryGetValue(machineId, out var machineActiveAlarms) && !machineActiveAlarms.IsEmpty)
            {
                foreach (var alarmNumber in machineActiveAlarms.Keys)
                {
                    if (_alarmDefinitionsCache.TryGetValue(alarmNumber, out var alarmDef))
                    {
                        activeAlarms.Add(alarmDef);
                    }
                }
            }
            // Alarmları önem seviyesine göre sıralayarak döndür
            return activeAlarms.OrderByDescending(a => a.Severity).ThenBy(a => a.AlarmNumber).ToList();
        }
        public Dictionary<int, IPlcManager> GetPlcManagers()
        {
            return new Dictionary<int, IPlcManager>(_plcManagers);
        }
    }
}