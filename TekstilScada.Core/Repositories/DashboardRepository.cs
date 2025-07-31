using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TekstilScada.Models;
using TekstilScada.Core; // Bu satırı ekleyin
namespace TekstilScada.Repositories
{
    public class DashboardRepository
    {
        private readonly string _connectionString = AppConfig.ConnectionString;

        public List<OeeData> GetOeeReport(DateTime startTime, DateTime endTime, int? machineId)
        {
            var oeeList = new List<OeeData>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                // Sadece tamamlanmış ve OEE için gerekli verileri olan batch'leri alıyoruz
                string query = @"
            SELECT 
                m.MachineName,
                b.BatchId,
                b.TotalProductionCount,
                b.DefectiveProductionCount,
                b.TotalDownTimeSeconds,
                b.StandardCycleTimeMinutes,
                TIME_TO_SEC(TIMEDIFF(b.EndTime, b.StartTime)) as PlannedTimeInSeconds
            FROM production_batches AS b
            JOIN machines AS m ON b.MachineId = m.Id
            WHERE 
                b.StartTime BETWEEN @StartTime AND @EndTime 
                AND b.EndTime IS NOT NULL 
                AND b.TotalProductionCount > 0 " + // Hesaplama için üretim olmalı
                    (machineId.HasValue ? "AND b.MachineId = @MachineId " : "") +
                    "ORDER BY m.MachineName;";

                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                if (machineId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@MachineId", machineId.Value);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double plannedTime = reader.IsDBNull(reader.GetOrdinal("PlannedTimeInSeconds")) ? 0 : reader.GetDouble("PlannedTimeInSeconds");
                        double downTime = reader.IsDBNull(reader.GetOrdinal("TotalDownTimeSeconds")) ? 0 : reader.GetDouble("TotalDownTimeSeconds");
                        double runTime = plannedTime - downTime;

                        int totalCount = reader.IsDBNull(reader.GetOrdinal("TotalProductionCount")) ? 0 : reader.GetInt32("TotalProductionCount");
                        int defectiveCount = reader.IsDBNull(reader.GetOrdinal("DefectiveProductionCount")) ? 0 : reader.GetInt32("DefectiveProductionCount");
                        int goodCount = totalCount - defectiveCount;

                        double standardCycleTime = reader.IsDBNull(reader.GetOrdinal("StandardCycleTimeMinutes")) ? 0 : (reader.GetDouble("StandardCycleTimeMinutes") * 60); // saniyeye çevir

                        // 1. Availability (Kullanılabilirlik)
                        double availability = (plannedTime > 0) ? (runTime / plannedTime) * 100 : 0;

                        // 2. Performance (Performans)
                        double performance = (runTime > 0 && totalCount > 0 && standardCycleTime > 0) ? (standardCycleTime * totalCount) / runTime * 100 : 0;

                        // 3. Quality (Kalite)
                        double quality = (totalCount > 0) ? ((double)goodCount / totalCount) * 100 : 0;

                        // OEE
                        double oee = (availability / 100) * (performance / 100) * (quality / 100) * 100;

                        oeeList.Add(new OeeData
                        {
                            MachineName = reader.GetString("MachineName"),
                            BatchId = reader.GetString("BatchId"),
                            Availability = Math.Max(0, Math.Round(availability, 2)), // Negatif ve ondalıklı sonuçları engelle
                            Performance = Math.Max(0, Math.Round(performance, 2)),
                            Quality = Math.Max(0, Math.Round(quality, 2)),
                            OEE = Math.Max(0, Math.Round(oee, 2))
                        });
                    }
                }
            }
            return oeeList;
        }
        public DataTable GetHourlyFactoryConsumption(DateTime date)
        {
            var dt = new DataTable();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                // Bu sorgu, tamamlanmış batch'lerin verilerini saatlik olarak gruplayıp toplar.
                string query = @"
                    SELECT 
                        HOUR(EndTime) AS Saat,
                        SUM(TotalElectricity) AS ToplamElektrik,
                        SUM(TotalWater) AS ToplamSu,
                        SUM(TotalSteam) AS ToplamBuhar
                    FROM production_batches
                    WHERE DATE(EndTime) = @SelectedDate
                    GROUP BY HOUR(EndTime)
                    ORDER BY Saat;
                ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@SelectedDate", date.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        // YENİ: Seçilen güne ait en çok tüketim yapan 5 makineyi getiren metot
        public DataTable GetTop5ConsumingMachines(DateTime date, string consumptionType)
        {
            var dt = new DataTable();
            string consumptionColumn = "";

            // Gelen parametreye göre SQL'deki sütun adını güvenli bir şekilde belirle
            switch (consumptionType.ToLower())
            {
                case "su":
                    consumptionColumn = "TotalWater";
                    break;
                case "buhar":
                    consumptionColumn = "TotalSteam";
                    break;
                default:
                    consumptionColumn = "TotalElectricity";
                    break;
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = $@"
                    SELECT 
                        m.MachineName,
                        SUM({consumptionColumn}) AS ToplamTuketim
                    FROM production_batches b
                    JOIN machines m ON b.MachineId = m.Id
                    WHERE DATE(b.EndTime) = @SelectedDate
                    GROUP BY m.MachineName
                    ORDER BY ToplamTuketim DESC
                    LIMIT 5;
                ";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@SelectedDate", date.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }
        // Bu metot şimdilik kullanılmıyor, olduğu gibi bırakabiliriz.
        public List<ProductionStepDetail> GetRecipeStepAnalysis(string recipeName)
        {
            var analysis = new List<ProductionStepDetail>();
            analysis.Add(new ProductionStepDetail { StepNumber = 1, StepName = "Su Alma", WorkingTime = "00:10:05", StopTime = "00:00:30" });
            analysis.Add(new ProductionStepDetail { StepNumber = 2, StepName = "Isıtma", WorkingTime = "00:25:15", StopTime = "00:01:00" });
            return analysis;
        }
    }
}