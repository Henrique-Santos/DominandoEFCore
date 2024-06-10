using DominandoEFCore11.Domain;
using DominandoEFCore11.Interceptadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore11.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Funcao> Funcoes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore07;Integrated Security=true;MultipleActiveResultSets=true;";

        optionsBuilder
            .UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .AddInterceptors(new InterceptadorDeComando())
            .AddInterceptors(new InterceptadorDeConexao())
            .AddInterceptors(new InterceptadorDePersistencia());
    }
}