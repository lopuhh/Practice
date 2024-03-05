
namespace Practice.Models;

public partial class AbonentService
{
    public DateTime Timestamp { get; set; }

    public short Abonent { get; set; }

    public string Service { get; set; } = null!;

    public string? Duration { get; set; }

    public decimal Cost { get; set; }

    public decimal CostNds { get; set; }

    public virtual Abonent AbonentNavigation { get; set; } = null!;
}
