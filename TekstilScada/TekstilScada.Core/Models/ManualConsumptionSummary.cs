namespace TekstilScada.Models
{
    public class ManualConsumptionSummary
    {
        public string Makine { get; set; }
        public string RaporAraligi { get; set; }
        public string ToplamManuelSure { get; set; }
        public double OrtalamaSicaklik { get; set; }
        public double OrtalamaDevir { get; set; }
        public int ToplamSuTuketimi_Litre { get; set; }
        public int ToplamElektrikTuketimi_kW { get; set; }
        public int ToplamBuharTuketimi_kg { get; set; }
    }
}