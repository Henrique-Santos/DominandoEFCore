using DominandoEFCore19.Domain;
using DominandoEFCore19.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore19.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Colaborador> Colaboradores { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<UsuarioFuncao> UsuarioFuncoes { get; set; }
    public DbSet<DepartamentoRelatorio> DepartamentoRelatorio { get; set; }

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
        // Informado que a tabela não possui PK com fluent api. Substitui a anotação [Keyless]
        //modelBuilder.Entity<UsuarioFuncao>().HasNoKey();

        // Indicando que esse mapeamento se refere a uma View e não a uma Table
        modelBuilder.Entity<DepartamentoRelatorio>(e =>
        {
            e.HasNoKey();

            e.ToView("vw_departamento_relatorio");

            e.Property(p => p.Departamento).HasColumnName("Descricao");
        });

        // Pegando todas as propriedades do tipo string que não possuem um tamanho definido
        var properties = modelBuilder.Model.GetEntityTypes()
            .SelectMany(p => p.GetProperties())
            .Where(p => p.ClrType == typeof(string) && p.GetColumnType() == null);

        foreach (var property in properties)
        {
            property.SetIsUnicode(false); // Definindo como VARCHAR
        }

        modelBuilder.ToSnakeCaseNames();
    }
}