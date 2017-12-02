namespace SavageTools
{
    public class MissionGeneratorSettings
    {
        public decimal DistancePerDay => Pace / 2.0M * 8M;
        public int Pace { get; set; } = 6;
        public bool UseHtml { get; set; }
        public int EventFrequency { get; set; } = 3;
    }
}
