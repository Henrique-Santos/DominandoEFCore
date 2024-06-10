using DominandoEFCore05a06.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore05a06.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore05a06;Integrated Security=true;MultipleActiveResultSets=true;";

            optionsBuilder
                //.UseSqlServer(connectionString, options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)) // (SplitQuery de forma Global) Para todas as queries que possuem relacionamentos ele irá dividir as consultas
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Server como um filtro global. Filtra automaticamente qualquer consulta na tabela Departamente trazendo apenas onde Departamento nao for Excluido */
            //modelBuilder.Entity<Departamento>().HasQueryFilter(d => !d.Excluido);
        }
    }
}