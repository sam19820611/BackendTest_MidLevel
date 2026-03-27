using Microsoft.EntityFrameworkCore;
using MyOfficeApi.Models;

namespace MyOfficeApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MyOfficeAcpd> MyOfficeAcpd { get; set; }
    public DbSet<MyOfficeExecutionLog> MyOfficeExecutionLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MyOfficeAcpd>(entity =>
        {
            entity.HasKey(e => e.AcpdSid);
            entity.Property(e => e.AcpdSid).IsFixedLength();
            entity.Property(e => e.AcpdStatus).HasDefaultValue((byte)0);
            entity.Property(e => e.AcpdStop).HasDefaultValue(false);
            entity.Property(e => e.AcpdNowDateTime).HasDefaultValueSql("getdate()");
            entity.Property(e => e.AcpdUpdDateTime).HasDefaultValueSql("getdate()");
        });

        modelBuilder.Entity<MyOfficeExecutionLog>(entity =>
        {
            entity.HasKey(e => e.DeLogAutoId);
            entity.Property(e => e.DeLogIsCustomDebug).HasDefaultValue(false);
            entity.Property(e => e.DeLogVerifyNeeded).HasDefaultValue(false);
            entity.Property(e => e.DeLogExDateTime).HasDefaultValueSql("getdate()");
        });
    }
}
