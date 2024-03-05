
namespace Practice.Models;

public partial class AbonentDetail
{
    public DateTime Timestamp { get; set; }

    public short Abonent { get; set; }

    public string Reporter { get; set; } = null!;

    public string Service { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public decimal Cost { get; set; }

    public string? Rouming { get; set; }

    public virtual Abonent AbonentNavigation { get; set; } = null!;
}
