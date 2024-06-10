using DominandoEFCore13.Domain;
using DominandoEFCore13.Funcoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DominandoEFCore13.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Livro> Livros { get; set; }

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
        UserDefinedFunctions.Registrar(modelBuilder);

        // Registrando UDFs atraves do fluent api
        modelBuilder
            .HasDbFunction(_left)
            .HasName("LEFT")
            .IsBuiltIn();

        modelBuilder
            .HasDbFunction(_letrasMaiusculas)
            .HasName("ConverterParaLetrasMaiusculas")
            .HasSchema("dbo");

        modelBuilder
            .HasDbFunction(_dateDiff)
            .HasName("DATEDIFF")
            .HasTranslation(p =>
            {
                // Convertendo o argumento DAY de string para constant
                var argumentos = p.ToList();

                var contante = (SqlConstantExpression)argumentos[0];
                argumentos[0] = new SqlFragmentExpression(contante.Value.ToString());

                return new SqlFunctionExpression("DATEDIFF", argumentos, false, [false, false, false], typeof(int), null);
            })
            .IsBuiltIn();
    }

    private static readonly MethodInfo _left = typeof(UserDefinedFunctions)
        .GetRuntimeMethod("Left", [typeof(string), typeof(int)]);

    private static readonly MethodInfo _letrasMaiusculas = typeof(UserDefinedFunctions)
        .GetRuntimeMethod(nameof(UserDefinedFunctions.LetrasMaiusculas), [typeof(string)]);

    private static readonly MethodInfo _dateDiff = typeof(UserDefinedFunctions)
        .GetRuntimeMethod(nameof(UserDefinedFunctions.DateDiff), [typeof(string), typeof(DateTime), typeof(DateTime)]);
}