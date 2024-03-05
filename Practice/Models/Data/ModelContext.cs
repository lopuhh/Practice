
using Microsoft.EntityFrameworkCore;

namespace Practice.Models.Data;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abonent> Abonents { get; set; }

    public virtual DbSet<AbonentDetail> AbonentDetails { get; set; }

    public virtual DbSet<AbonentService> AbonentServices { get; set; }

    public virtual DbSet<AbonentType> AbonentTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User ID=lopuh;Password=1234;Persist Security Info=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("LOPUH")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Abonent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008355");

            entity.ToTable("ABONENT");

            entity.Property(e => e.Id)
                .HasPrecision(5)
                .HasColumnName("ID");
            entity.Property(e => e.City)
                .HasPrecision(4)
                .HasColumnName("CITY");
            entity.Property(e => e.Country)
                .HasPrecision(3)
                .HasDefaultValueSql("24")
                .HasColumnName("COUNTRY");
            entity.Property(e => e.Description)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Fax)
                .HasPrecision(2)
                .HasColumnName("FAX");
            entity.Property(e => e.Pnumber)
                .HasPrecision(8)
                .HasColumnName("PNUMBER");
            entity.Property(e => e.Ptype)
                .HasPrecision(2)
                .HasDefaultValueSql("0")
                .HasColumnName("PTYPE");
            entity.Property(e => e.Secure)
                .HasPrecision(2)
                .HasColumnName("SECURE");

            entity.HasOne(d => d.PtypeNavigation).WithMany(p => p.Abonents)
                .HasForeignKey(d => d.Ptype)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ABONENT_TYPE_FC");
        });

        modelBuilder.Entity<AbonentDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ABONENT_DETAILS");

            entity.Property(e => e.Abonent)
                .HasPrecision(5)
                .HasColumnName("ABONENT");
            entity.Property(e => e.Cost)
                .HasColumnType("NUMBER")
                .HasColumnName("COST");
            entity.Property(e => e.Duration)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("DURATION");
            entity.Property(e => e.Reporter)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("REPORTER");
            entity.Property(e => e.Rouming)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("ROUMING");
            entity.Property(e => e.Service)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("SERVICE");
            entity.Property(e => e.Timestamp)
                .HasColumnType("DATE")
                .HasColumnName("TIMESTAMP#");

            entity.HasOne(d => d.AbonentNavigation).WithMany()
                .HasForeignKey(d => d.Abonent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ABONENT_FC");
        });

        modelBuilder.Entity<AbonentService>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ABONENT_SERVICES");

            entity.Property(e => e.Abonent)
                .HasPrecision(5)
                .HasColumnName("ABONENT");
            entity.Property(e => e.Cost)
                .HasColumnType("NUMBER")
                .HasColumnName("COST");
            entity.Property(e => e.CostNds)
                .HasColumnType("NUMBER")
                .HasColumnName("COST_NDS");
            entity.Property(e => e.Duration)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("DURATION");
            entity.Property(e => e.Service)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("SERVICE");
            entity.Property(e => e.Timestamp)
                .HasColumnType("DATE")
                .HasColumnName("TIMESTAMP#");

            entity.HasOne(d => d.AbonentNavigation).WithMany()
                .HasForeignKey(d => d.Abonent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ABONENT_FC2");
        });

        modelBuilder.Entity<AbonentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008353");

            entity.ToTable("ABONENT_TYPE");

            entity.Property(e => e.Id)
                .HasPrecision(2)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Mobile)
                .HasPrecision(2)
                .HasDefaultValueSql("0\n")
                .HasColumnName("MOBILE");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });
        modelBuilder.HasSequence("ABONENT_SEQUENCE");
        modelBuilder.HasSequence("ABONENT_TYPE_SEQUENCE");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
