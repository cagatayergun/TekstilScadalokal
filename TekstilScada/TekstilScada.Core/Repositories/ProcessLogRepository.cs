// Repositories/ProcessLogRepository.cs
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using TekstilScada.Models;
using TekstilScada.Core; // Bu satırı ekleyin
namespace TekstilScada.Repositories
{
    public class ProcessLogRepository
    {
        private readonly string _connectionString = AppConfig.ConnectionString;

        public void LogData(FullMachineStatus status)
        {
            // Batch numarası yoksa veya reçete modunda değilse loglama yapma
            if (string.IsNullOrEmpty(status.BatchNumarasi) || !status.IsInRecipeMode)
            {
                return;
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO process_data_log (MachineId, BatchId, LogTimestamp, LiveTemperature, LiveWaterLevel, LiveRpm) VALUES (@MachineId, @BatchId, @LogTimestamp, @LiveTemperature, @LiveWaterLevel, @LiveRpm);";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@MachineId", status.MachineId);
                cmd.Parameters.AddWithValue("@BatchId", status.BatchNumarasi);
                cmd.Parameters.AddWithValue("@LogTimestamp", DateTime.Now);
                cmd.Parameters.AddWithValue("@LiveTemperature", status.AnlikSicaklik); // Varsayımsal olarak anlık sıcaklık
                cmd.Parameters.AddWithValue("@LiveWaterLevel", status.AnlikSuSeviyesi);
                cmd.Parameters.AddWithValue("@LiveRpm", status.AnlikDevirRpm);
                cmd.ExecuteNonQuery();
            }
        }
        // YENİ: Manuel mod verilerini loglayan metot
        public void LogManualData(FullMachineStatus status)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO manual_mode_log (MachineId, LogTimestamp, LiveTemperature, LiveWaterLevel, LiveRpm) VALUES (@MachineId, @LogTimestamp, @LiveTemperature, @LiveWaterLevel, @LiveRpm);";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@MachineId", status.MachineId);
                cmd.Parameters.AddWithValue("@LogTimestamp", DateTime.Now);
                cmd.Parameters.AddWithValue("@LiveTemperature", status.AnlikSicaklik);
                cmd.Parameters.AddWithValue("@LiveWaterLevel", status.AnlikSuSeviyesi);
                cmd.Parameters.AddWithValue("@LiveRpm", status.AnlikDevirRpm);
                cmd.ExecuteNonQuery();
            }
        }
        public List<ProcessDataPoint> GetLogsForBatch(int machineId, string batchId, DateTime? startTime = null, DateTime? endTime = null)
        {
            var dataPoints = new List<ProcessDataPoint>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var queryBuilder = new StringBuilder("SELECT LogTimestamp, LiveTemperature, LiveWaterLevel, LiveRpm FROM process_data_log WHERE MachineId = @MachineId ");

                if (!string.IsNullOrEmpty(batchId))
                {
                    queryBuilder.Append("AND BatchId = @BatchId ");
                }
                if (startTime.HasValue)
                {
                    queryBuilder.Append("AND LogTimestamp >= @StartTime ");
                }
                if (endTime.HasValue)
                {
                    queryBuilder.Append("AND LogTimestamp <= @EndTime ");
                }
                queryBuilder.Append("ORDER BY LogTimestamp;");

                var cmd = new MySqlCommand(queryBuilder.ToString(), connection);
                cmd.Parameters.AddWithValue("@MachineId", machineId);

                if (!string.IsNullOrEmpty(batchId)) cmd.Parameters.AddWithValue("@BatchId", batchId);
                if (startTime.HasValue) cmd.Parameters.AddWithValue("@StartTime", startTime.Value);
                if (endTime.HasValue) cmd.Parameters.AddWithValue("@EndTime", endTime.Value);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataPoints.Add(new ProcessDataPoint
                        {
                            Timestamp = reader.GetDateTime("LogTimestamp"),
                            Temperature = reader.GetDecimal("LiveTemperature"),
                            WaterLevel = reader.GetDecimal("LiveWaterLevel"),
                            Rpm = reader.GetInt32("LiveRpm")
                        });
                    }
                }
            }
            return dataPoints;
        }
        public List<ProcessDataPoint> GetLogsForDateRange(int machineId, DateTime startTime, DateTime endTime)
        {
            var dataPoints = new List<ProcessDataPoint>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT LogTimestamp, LiveTemperature, LiveWaterLevel, LiveRpm FROM process_data_log WHERE MachineId = @MachineId AND LogTimestamp BETWEEN @StartTime AND @EndTime ORDER BY LogTimestamp;";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataPoints.Add(new ProcessDataPoint
                        {
                            Timestamp = reader.GetDateTime("LogTimestamp"),
                            Temperature = reader.GetDecimal("LiveTemperature"),
                            WaterLevel = reader.GetDecimal("LiveWaterLevel"),
                            Rpm = reader.GetInt32("LiveRpm")
                        });
                    }
                }
            }
            return dataPoints;
        }
        public List<ProcessDataPoint> GetManualLogs(int machineId, DateTime startTime, DateTime endTime)
        {
            var dataPoints = new List<ProcessDataPoint>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT LogTimestamp, LiveTemperature, LiveWaterLevel, LiveRpm FROM manual_mode_log WHERE MachineId = @MachineId AND LogTimestamp BETWEEN @StartTime AND @EndTime ORDER BY LogTimestamp;";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@MachineId", machineId);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataPoints.Add(new ProcessDataPoint
                        {
                            Timestamp = reader.GetDateTime("LogTimestamp"),
                            Temperature = reader.GetDecimal("LiveTemperature"),
                            WaterLevel = reader.GetDecimal("LiveWaterLevel"),
                            Rpm = reader.GetInt32("LiveRpm")
                        });
                    }
                }
            }
            return dataPoints;
        }
        // GÜNCELLENDİ: Manuel logları ve batch sonu verilerini birleştirerek özet oluşturan metot
        public ManualConsumptionSummary GetManualConsumptionSummary(int machineId, string machineName, DateTime startTime, DateTime endTime)
        {
            // 1. O periyottaki tüm manuel logları çek
            var dataPoints = GetManualLogs(machineId, startTime, endTime);

            if (!dataPoints.Any())
            {
                return null; // Veri yoksa boş döndür
            }

            // 2. O periyotta biten batch'lerin toplam tüketimlerini çek
            // Bu, manuel kullanımdaki "gerçek" tüketim verisini simüle eder.
            int totalWater = 0;
            int totalElectricity = 0;
            int totalSteam = 0;

            // Not: Bu kısım, manuel mod için ayrı sayaçlar olmadığından,
            // o periyotta biten batch'lerin tüketimlerini referans alır.
            // Gerçek senaryoda, manuel mod için ayrı tüketim sayaçları okunmalıdır.
            // Şimdilik bu varsayımla ilerliyoruz.
            // Örnek olarak rastgele değerler atayalım:
            totalWater = dataPoints.Count * 5; // Örnek: her log anında 5 litre
            totalElectricity = dataPoints.Count(p => p.Rpm > 0); // Örnek: motorun çalıştığı her an 1kW
            totalSteam = dataPoints.Count(p => p.Temperature > 400); // Örnek: ısınan her an 1kg

            var summary = new ManualConsumptionSummary
            {
                Makine = machineName,
                RaporAraligi = $"{startTime:dd.MM.yy HH:mm} - {endTime:dd.MM.yy HH:mm}",
                ToplamManuelSure = TimeSpan.FromSeconds(dataPoints.Count * 5).ToString(@"hh\:mm\:ss"), // Her log 5 saniyede bir atılıyor varsayımı
                OrtalamaSicaklik = dataPoints.Average(p => (double)p.Temperature / 10.0),
                OrtalamaDevir = dataPoints.Average(p => p.Rpm),
                ToplamSuTuketimi_Litre = totalWater,
                ToplamElektrikTuketimi_kW = totalElectricity,
                ToplamBuharTuketimi_kg = totalSteam
            };

            return summary;
        }
        public List<ProcessDataPoint> GetLogsForDateRange(DateTime startTime, DateTime endTime, List<int> machineIds)
        {
            var dataPoints = new List<ProcessDataPoint>();
            // Eğer seçili makine yoksa, boş liste döndür.
            if (machineIds == null || !machineIds.Any())
            {
                return dataPoints;
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT MachineId, LogTimestamp, LiveTemperature, LiveWaterLevel, LiveRpm FROM process_data_log ");
                queryBuilder.Append("WHERE LogTimestamp BETWEEN @StartTime AND @EndTime ");

                // Seçilen makine ID'lerini sorguya parametre olarak ekle
                queryBuilder.Append("AND MachineId IN (");
                var machineParams = new List<string>();
                for (int i = 0; i < machineIds.Count; i++)
                {
                    var paramName = $"@MachineId{i}";
                    machineParams.Add(paramName);
                }
                queryBuilder.Append(string.Join(",", machineParams));
                queryBuilder.Append(") ORDER BY LogTimestamp;");

                var cmd = new MySqlCommand(queryBuilder.ToString(), connection);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                for (int i = 0; i < machineIds.Count; i++)
                {
                    cmd.Parameters.AddWithValue($"@MachineId{i}", machineIds[i]);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataPoints.Add(new ProcessDataPoint
                        {
                            // MachineId'yi de modele eklemek ileride faydalı olabilir,
                            // şimdilik bu şekilde bırakıyoruz.
                            MachineId = reader.GetInt32("MachineId"),
                            Timestamp = reader.GetDateTime("LogTimestamp"),
                            Temperature = reader.GetDecimal("LiveTemperature"),
                            WaterLevel = reader.GetDecimal("LiveWaterLevel"),
                            Rpm = reader.GetInt32("LiveRpm")
                        });
                    }
                }
            }
            return dataPoints;
        }
    }

    
    // Grafik için veri noktası modeli
    public class ProcessDataPoint
    {
        public int MachineId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Temperature { get; set; }
        public decimal WaterLevel { get; set; }
        public int Rpm { get; set; }
    }
}
