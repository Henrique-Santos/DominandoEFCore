using DominandoEFCore07.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore07.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly StreamWriter _writer = 
            new($"{Environment.CurrentDirectory.Remove(Environment.CurrentDirectory.IndexOf("bin"))}meu_log_do_ef_core.txt", append: true);

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore07;Integrated Security=true;MultipleActiveResultSets=true;";

            optionsBuilder
                .UseSqlServer(connectionString)
                //.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()) // Adiciona resiliencia a consulta, o número de tentativas padrao desse método é 6
                //.UseSqlServer(connectionString, options => options.CommandTimeout(5)) // Diminuindo o timeout do ef-core
                //.UseSqlServer(connectionString, options => options.MaxBatchSize(100)) // Alterando o tamanho de itens de insersao por lote
                .EnableDetailedErrors() // Mostra mais detalhes de um erro
                //.LogTo(_writer.WriteLine); // Registra os logs do ef-core no arquivo de texto
                //.LogTo(Console.WriteLine, new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted }); // É possivel filtrar os logs gerados pelo ef-core com base nos eventos
                .LogTo(Console.WriteLine, LogLevel.Information) // O ef-core irá logar todos os logs no console da app que forem do tipo Information
                .EnableSensitiveDataLogging() // Loga os dados dos parametros das queries
                ;
        }

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}