

namespace Practice.Models.Report
{
    public partial class Report3
    {
        public int Pnumber { get; set; }
        public string Name { get; set; }
        public string Service { get; set; }
        public int Timestamp { get; set; }
        public string? Duration { get; set; }
        public decimal Cost { get; set; }
        public decimal CostNds { get; set; }
        public decimal SumCost { get; set; }
    }
}
