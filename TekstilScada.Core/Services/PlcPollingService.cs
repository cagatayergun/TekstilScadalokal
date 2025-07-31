// Services/PlcPollingService.cs
//using Microsoft.AspNetCore.SignalR; // SignalR için eklendi
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TekstilScada.Models;
using TekstilScada.Repositories;
// WebAPI projesindeki Hub'a erişebilmek için bir using ifadesi ekliyoruz.
// Bu, normalde katmanlar arası istenmeyen bir durumdur ancak bu mimaride
// en pratik çözümdür.
//using TekstilScada.WebAPI.Hubs;

namespace TekstilScada.Services
{
    public class PlcPollingService
    {   
        // === YENİ ALANLAR ===
        // SignalR Hub'ına erişim sağlamak için bir HubContext ekliyoruz.
        //private readonly IHubContext<ScadaHub> _hubContext;
        // DEĞİŞİKLİK: LsPlcManager -> IPlcManager
        private ConcurrentDictionary<int, IPlcManager> _plcManagers;
        public ConcurrentDictionary<int, FullMachineStatus> MachineDataCache { get; private set; }

        private ConcurrentDictionary<int, ConcurrentDictionary<int, DateTime>> _activeAlarmsTracker;
        private ConcurrentDictionary<int, AlarmDefinition> _alarmDefinitionsCache;

        private readonly AlarmRepository _alarmRepository;
        private readonly ProcessLogRepository _processLogRepository;
        private ConcurrentDictionary<int, string> _currentBatches;
        private ConcurrentDictionary<int, DateTime> _reconnectAttempts;
        private ConcurrentDictionary<int, ConnectionStatus> _connectionStates;

        private System.Threading.Timer _mainPollingTimer;
        private readonly int _pollingIntervalMs = 1000;
        private readonly int _loggingIntervalMs = 5000;
        private System.Threading.Timer _loggingTimer;
        private readonly ProductionRepository _productionRepository;
        private readonly RecipeRepository _recipeRepository;
        private ConcurrentDictionary<int, short> _lastKnownStepNumbers;

        public event Action<int, FullMachineStatus> OnMachineDataRefreshed;
        public event Action<int, FullMachineStatus> OnMachineConnectionStateChanged;
        public event Action<int, FullMachineStatus> OnActiveAlarmStateChanged;


        public PlcPollingService()
        {
           // _hubContext = hubContext;
            // DEĞİŞİKLİK: IPlcManager uyumlu hale getirildi
            _plcManagers = new ConcurrentDictionary<int, IPlcManager>();
            MachineDataCache = new ConcurrentDictionary<int, FullMachineStatus>();
            _reconnectAttempts = new ConcurrentDictionary<int, DateTime>();
            _connectionStates = new ConcurrentDictionary<int, ConnectionStatus>();

            _activeAlarmsTracker = new ConcurrentDictionary<int, ConcurrentDictionary<int, DateTime>>();
            _alarmRepository = new AlarmRepository();
            _processLogRepository = new ProcessLogRepository();
            _currentBatches = new ConcurrentDictionary<int, string>();
            _productionRepository = new ProductionRepository();
            _recipeRepository = new RecipeRepository();
            _lastKnownStepNumbers = new ConcurrentDictionary<int, short>();
        }

        public void Start(List<Machine> machines)
        {
            Stop();
            LoadAlarmDefinitionsCache();

            foreach (var machine in machines)
            {
                try
                {
                    // YENİ: Makine tipine göre doğru yöneticiyi Fabrika'dan al
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
                    LiveEventAggregator.Instance.Publish(new LiveEvent
                    {
                        Type = EventType.SystemWarning,
                        Source = machine.MachineName,
                        Message = $"Makine başlatılamadı: {ex.Message}"
                    });
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

        private void PollingTimer_Tick(object state)
        {
            Parallel.ForEach(_plcManagers, kvp =>
            {
                int machineId = kvp.Key;
                IPlcManager manager = kvp.Value; // Değişiklik: LsPlcManager -> IPlcManager
                if (_connectionStates.TryGetValue(machineId, out var currentState) && currentState != ConnectionStatus.Connected)
                {
                    // Sadece anahtar mevcutsa ve durumu "Connected" değilse yeniden bağlanmayı dene
                    HandleReconnection(machineId, manager);
                }

             
                else
                {
                    var readResult = manager.ReadLiveStatusData();
                    if (readResult.IsSuccess)
                    {
                        var status = readResult.Content;
                        status.MachineId = machineId;
                        status.MachineName = MachineDataCache[machineId].MachineName;
                        CheckAndLogBatch(machineId, status);
                        CheckAndLogAlarms(machineId, status);

                        MachineDataCache[machineId] = status;
                        OnMachineDataRefreshed?.Invoke(machineId, status);
                        // === YENİ KOD ===
                        // Veri güncellendiğinde, bu veriyi SignalR üzerinden tüm istemcilere gönder.
                 //       _hubContext.Clients.All.SendAsync("ReceiveMachineUpdate", status);
                        // ================
                    }
                    else
                    {
                        HandleDisconnection(machineId);
                    }
                }
            });
        }

        // DEĞİŞİKLİK: LsPlcManager -> IPlcManager
        public Dictionary<int, IPlcManager> GetPlcManagers()
        {
            return new Dictionary<int, IPlcManager>(_plcManagers);
        }

        // Diğer metotlar (CheckAndLogBatch, CheckAndLogAlarms, HandleDisconnection vb.)
        // arayüz (interface) üzerinden çalıştığı için herhangi bir değişiklik gerektirmez.
        // Bu metotların tam ve güncel hallerini aşağıya ekliyorum.

        private void LoadAlarmDefinitionsCache()
        {
            try
            {
                var definitions = _alarmRepository.GetAllAlarmDefinitions();
                _alarmDefinitionsCache = new ConcurrentDictionary<int, AlarmDefinition>(
                    definitions.ToDictionary(def => def.AlarmNumber, def => def)
                );
                LiveEventAggregator.Instance.PublishSystemInfo("Alarm tanımları önbelleğe alındı.");
            }
            catch (Exception ex)
            {
                _alarmDefinitionsCache = new ConcurrentDictionary<int, AlarmDefinition>();
                LiveEventAggregator.Instance.Publish(new LiveEvent
                {
                    Type = EventType.SystemWarning,
                    Source = "Sistem",
                    Message = $"Alarm tanımları önbelleğe alınamadı: {ex.Message}"
                });
            }
        }

        private void CheckAndLogBatch(int machineId, FullMachineStatus currentStatus)
        {
            _currentBatches.TryGetValue(machineId, out string lastBatchId);

            // Yeni bir batch başlıyorsa...
            if (currentStatus.IsInRecipeMode && !string.IsNullOrEmpty(currentStatus.BatchNumarasi) && currentStatus.BatchNumarasi != lastBatchId)
            {
                _productionRepository.StartNewBatch(currentStatus);
                _currentBatches[machineId] = currentStatus.BatchNumarasi;
            }
            // Mevcut batch bitiyorsa...
            else if (!currentStatus.IsInRecipeMode && lastBatchId != null)
            {
                // Üretim bittiği anda PLC'den son verileri alıyoruz.
                // currentStatus, bu verileri zaten içinde barındırıyor.
                _productionRepository.EndBatch(machineId, lastBatchId, currentStatus);

                // Diğer rapor verilerini asenkron olarak çekmeye devam et
                if (_plcManagers.TryGetValue(machineId, out var plcManager))
                {
                    Task.Run(async () => {
                        // ... (mevcut summary, chemical, step analysis kodları aynı kalacak) ...

                        // YENİ: Üretim sayacını 1 artır
                        await plcManager.IncrementProductionCounterAsync();

                        // YENİ: Diğer OEE sayaçlarını sıfırla
                        await plcManager.ResetOeeCountersAsync();

                        LiveEventAggregator.Instance.PublishSystemInfo($"{lastBatchId} için üretim tamamlandı. Sayaçlar sıfırlandı.");
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

            if (currentAlarmNumber > 0)
            {
                if (!machineActiveAlarms.ContainsKey(currentAlarmNumber) && _alarmDefinitionsCache.TryGetValue(currentAlarmNumber, out var newAlarmDef))
                {
                    _alarmRepository.WriteAlarmHistoryEvent(machineId, newAlarmDef.Id, "ACTIVE");
                    LiveEventAggregator.Instance.PublishAlarm(currentStatus.MachineName, newAlarmDef.AlarmText);
                }
                machineActiveAlarms[currentAlarmNumber] = DateTime.Now;
            }

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

            currentStatus.HasActiveAlarm = !machineActiveAlarms.IsEmpty;
            if (currentStatus.HasActiveAlarm)
            {
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

            if ((previousStatus?.HasActiveAlarm ?? false) != currentStatus.HasActiveAlarm || previousAlarmNumber != currentStatus.ActiveAlarmNumber)
            {
                OnActiveAlarmStateChanged?.Invoke(machineId, currentStatus);
            }
        }

        private void HandleDisconnection(int machineId)
        {
            var status = MachineDataCache[machineId];
            status.ConnectionState = ConnectionStatus.ConnectionLost;
            _connectionStates[machineId] = ConnectionStatus.ConnectionLost;
            _reconnectAttempts.TryAdd(machineId, DateTime.UtcNow);
            OnMachineConnectionStateChanged?.Invoke(machineId, status);

            LiveEventAggregator.Instance.Publish(new LiveEvent
            {
                Timestamp = DateTime.Now,
                Source = status.MachineName,
                Message = "İletişim koptu!",
                Type = EventType.SystemWarning
            });
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
                if (machineStatus.ConnectionState == ConnectionStatus.Connected && machineStatus.IsInRecipeMode)
                {
                    try
                    {
                        // Eğer makine üretim modundaysa, üretim logu at.
                        if (machineStatus.IsInRecipeMode)
                        {
                            _processLogRepository.LogData(machineStatus);
                        }
                        // Eğer makine üretimde DEĞİLSE ama tüketim yapıyorsa (ısı veya motor), manuel log at.
                        else if (machineStatus.AnlikSicaklik > 30 || machineStatus.AnlikDevirRpm > 0) // 30°C ortam sıcaklığı varsayımı
                        {
                            _processLogRepository.LogManualData(machineStatus);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Makine {machineStatus.MachineId} için veri loglama hatası: {ex.Message}");
                    }
                }
            }
        }
    }
}
