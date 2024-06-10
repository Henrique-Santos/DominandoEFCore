using DominandoEFCore09.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore09.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Atributo> Atributos { get; set; }

    public DbSet<Aeroporto> Aeroportos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore07;Integrated Security=true;MultipleActiveResultSets=true;";

        optionsBuilder
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }
}