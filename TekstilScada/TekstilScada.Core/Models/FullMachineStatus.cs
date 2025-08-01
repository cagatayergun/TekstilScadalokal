﻿// Models/FullMachineStatus.cs
namespace TekstilScada.Models
{
    /// <summary>
    /// Bir makinenin tüm anlık durumunu tek bir nesnede birleştiren sınıf.
    /// UI katmanını beslemek için kullanılır.
    /// </summary>
    public class FullMachineStatus
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public ConnectionStatus ConnectionState { get; set; }

        // Anlık Proses Değerleri
        public short AnlikSuSeviyesi { get; set; }
        public short AnlikDevirRpm { get; set; }
        public short AnlikSicaklik { get; set; }
        public short ProsesYuzdesi { get; set; }

        // Durum Bayrakları
        public bool IsInRecipeMode { get; set; }
        public bool IsPaused { get; set; }
        public bool HasActiveAlarm { get; set; }
        public int ActiveAlarmNumber { get; set; }
        public string ActiveAlarmText { get; set; }

        // İş Emri ve Tanımlayıcı Bilgiler
        public string MakineTipi { get; set; }
        public string SiparisNumarasi { get; set; }
        public string MusteriNumarasi { get; set; }
        public string BatchNumarasi { get; set; }
        public string OperatorIsmi { get; set; }
        // YENİ: Reçete Adı özelliği eklendi.
        public string RecipeName { get; set; }

        // Aktif Adım Bilgileri
        public short AktifAdimNo { get; set; }
        public string AktifAdimAdi { get; set; }
        public short SuMiktari { get; set; }
        public short ElektrikHarcama { get; set; }
        public short BuharHarcama { get; set; }

        // Aktif Adım Bilgileri
        public bool IsMachineInProduction { get; set; } // M2501
        public int TotalDownTimeSeconds { get; set; }     // D7764
        public short StandardCycleTimeMinutes { get; set; } // D6411
        public short TotalProductionCount { get; set; }   // D7768
        public short DefectiveProductionCount { get; set; } // D7770

        // YENİ
        public int CalismaSuresiDakika { get; set; }
    }

    public enum ConnectionStatus
    {
        Disconnected,
        Connecting,
        Connected,
        ConnectionLost
    }
}
