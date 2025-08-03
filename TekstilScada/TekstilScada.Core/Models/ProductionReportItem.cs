// Models/ProductionReportItem.cs
using System;

namespace TekstilScada.Models
{
    public class ProductionReportItem
    {
        // YENİ: Rapor detayları için gerekli olan alanlar eklendi.
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public string RecipeName { get; set; }
        public string OperatorName { get; set; }
        public string MusteriNo { get; set; }
        public string SiparisNo { get; set; }
        public string BatchId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CycleTime { get; set; }
        public decimal AverageTemperature { get; set; }
        public decimal MaxWaterLevel { get; set; }
        public double AverageRpm { get; set; }
        public int TotalWater { get; set; }
        public int TotalElectricity { get; set; }
        public int TotalSteam { get; set; }
       
            public double MachineAlarmDurationSeconds { get; set; }
        public double OperatorPauseDurationSeconds { get; set; }
    }
}
