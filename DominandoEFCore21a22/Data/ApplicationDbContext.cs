using DominandoEFCore21a22.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DominandoEFCore21a22.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore21;Integrated Security=true;MultipleActiveResultSets=true;";

        optionsBuilder
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine)
            .UseSqlServer(connectionString)
            .ReplaceService<IQuerySqlGeneratorFactory, CustomSqlServerQuerySqlGeneratorFactory>();
    }
}