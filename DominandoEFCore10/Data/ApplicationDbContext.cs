using DominandoEFCore10.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore10.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Funcao> Funcoes { get; set; }

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
        modelBuilder.Entity<Funcao>(opt =>
        {
            opt.Property<string>("PropriedadeDeSombra")
                .HasColumnType("VARCHAR(100)")
                .HasDefaultValueSql("'Teste'");
        });
    }
}