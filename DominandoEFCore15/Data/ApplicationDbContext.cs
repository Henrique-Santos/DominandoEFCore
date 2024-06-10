using DominandoEFCore15.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore15.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Pessoa> Pessoas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore07;Integrated Security=true;MultipleActiveResultSets=true;";

        optionsBuilder
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(opt =>
        {
            opt.HasKey(p => p.Id);
            opt.Property(p => p.Nome)
                .HasMaxLength(60)
                .IsUnicode(false); // VARCHAR ao invés de NVARCHAR
        });
    }
}