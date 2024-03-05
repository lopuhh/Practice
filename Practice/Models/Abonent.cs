

namespace Practice.Models;

public partial class Abonent
{
    public short Id { get; set; }

    public byte Country { get; set; }

    public byte City { get; set; }

    public int Pnumber { get; set; }

    public byte? Fax { get; set; }

    public string? Description { get; set; }

    public byte Ptype { get; set; }

    public byte? Secure { get; set; }

    public virtual AbonentType PtypeNavigation { get; set; } = null!;
}
