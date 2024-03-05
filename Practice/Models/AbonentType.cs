
namespace Practice.Models;

public partial class AbonentType
{
    public byte Id { get; set; }

    public string? Name { get; set; }

    public byte Mobile { get; set; }

    public virtual ICollection<Abonent> Abonents { get; set; } = new List<Abonent>();
}
