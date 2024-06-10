using DominandoEFCore01a04.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Diagnostics;

namespace DominandoEFCore01a04
{
    public class Program
    {
        static int _count;

        public static void Main(string[] args)
        {
            /* ---------------- Funcionalidades ------------------------ */

            //EnsureCreatedAndDeleted();
            //GapDoEnsureCreated();
            //HealthCheckBancoDeDados();
            //ExecutarGerenciamentoDeEstado();
            //ExecuteSQL();
            //SQLInjection();
            //MigracaoPendentes();
            //AplicaMigracaoEmTempoDeExecucao();
            //TodasAsMigracoes();
            //MigracoesJaAplicadas();
            //ScriptGeralDoBD();

            /* ---------------- Tipos de Carregamento ------------------------ */

            //CarregamentoAdiantado();
            //CarregamentoExplicito();
            //CarregamentoLento();
        }

        public static void EnsureCreatedAndDeleted()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }

        // O EnsureCreated só é executado se não existir tabelas no banco de dados
        // Por isso quando existe mais de um contexto é necessario pedir para criar as tabelas
        public static void GapDoEnsureCreated()
        {
            using var db1 = new ApplicationDbContext();
            using var db2 = new OtherDbContext();

            db1.Database.EnsureCreated(); 
            db2.Database.EnsureCreated();

            var dbCreator = db2.GetService<IRelationalDatabaseCreator>();
            dbCreator.CreateTables();
        }

        public static void HealthCheckBancoDeDados()
        {
            using var db = new ApplicationDbContext();

            var canConnect = db.Database.CanConnect();

            if (canConnect) 
                Console.WriteLine("Posso me conectar");
            else 
                Console.WriteLine("Não posso me conectar");
        }

        public static void ExecutarGerenciamentoDeEstado()
        {
            /* warmup */
            new ApplicationDbContext().Departamentos.Any();
            _count = 0;
            GerenciarEstadoDaConexao(false);
            _count = 0;
            GerenciarEstadoDaConexao(true);
        }

        public static void GerenciarEstadoDaConexao(bool gerenciar)
        {
            using var db = new ApplicationDbContext();

            var time = Stopwatch.StartNew();

            var conexao = db.Database.GetDbConnection();

            conexao.StateChange += (_, __) => ++_count;

            if (gerenciar) conexao.Open(); // Abrindo conexão prematuramente

            for (int i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            if (gerenciar) conexao.Close();

            time.Stop();

            var mensagem = $"Tempo: {time.Elapsed}, {gerenciar}, Contador: {_count}";

            Console.WriteLine(mensagem);
        }

        // As execuções de comandos SQL não são rastreadas pelo ef-core
        public static void ExecuteSQL()
        {
            using var db = new ApplicationDbContext();

            // 1º Opção
            using var cmd = db.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "SELECT 1";
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            
            // 2º Opção
            var descricao = "Teste";
            db.Database.ExecuteSqlRaw("UPDATE Departamentos SET Descricao = {0} WHERE Id = 1", descricao);

            // 3º Opção
            db.Database.ExecuteSqlInterpolated($"UPDATE Departamentos SET Descricao = {descricao} WHERE Id = 1");
        }

        public static void SQLInjection()
        {
            using var db = new ApplicationDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Domain.Departamento
                {
                    Descricao = "Departamento 01"
                },
                new Domain.Departamento
                {
                    Descricao = "Departamento 02"
                }
            );

            db.SaveChanges();

            var descricao = "Teste ' OR 1 ='1";
            db.Database.ExecuteSqlRaw($"UPDATE Departamentos SET Descricao = 'AtaqueSQLInjection' WHERE Descricao = '{descricao}'");

            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, Descrição: {departamento.Descricao}");
            }
        }

        public static void MigracaoPendentes()
        {
            using var db = new ApplicationDbContext();

            var migracoesPendentes = db.Database.GetPendingMigrations();

            Console.WriteLine($"Total: {migracoesPendentes.Count()}");

            foreach (var migracao in migracoesPendentes)
            {
                Console.WriteLine($"Migracao: {migracao}");
            }
        }

        // Não é recomendado aplicar migraçoes dessa maneira
        public static void AplicaMigracaoEmTempoDeExecucao()
        {
            using var db1 = new ApplicationDbContext();

            db1.Database.Migrate();

            using var db2 = new OtherDbContext();

            db2.Database.Migrate();
        }

        public static void TodasAsMigracoes()
        {
            using var db = new ApplicationDbContext();

            var migracoes = db.Database.GetMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migracao: {migracao}");
            }

        }

        // Pega de todos os contextos
        public static void MigracoesJaAplicadas()
        {
            using var db = new ApplicationDbContext();

            var migracoes = db.Database.GetAppliedMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migracao: {migracao}");
            }
        }

        public static void ScriptGeralDoBD()
        {
            using var db = new ApplicationDbContext();

            var script = db.Database.GenerateCreateScript();

            Console.Write(script);
        }

        // -----

        public static void SetupTiposCarregamentos(ApplicationDbContext db)
        {
            if (!db.Departamentos.Any())
            {
                db.Departamentos.AddRange(
                    new Domain.Departamento
                    {
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Domain.Funcionario>
                        {
                             new() {
                                 Nome = "Rafael",
                                 Cpf = "99999999911"
                             }
                        }
                    },
                    new Domain.Departamento
                    {
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Domain.Funcionario>
                        {
                             new() {
                                 Nome = "Bruno",
                                 Cpf = "99999999922"
                             },
                             new() {
                                 Nome = "Eduardo",
                                 Cpf = "99999999933"
                             }
                        }
                    }
                );

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

        public static void CarregamentoAdiantado()
        {
            using var db = new ApplicationDbContext();

            SetupTiposCarregamentos(db);

            var departamentos = db.Departamentos.Include(d => d.Funcionarios);

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionario encontrado");
                }
            }
        }

        public static void CarregamentoExplicito()
        {
            using var db = new ApplicationDbContext();

            SetupTiposCarregamentos(db);

            var departamentos = db.Departamentos;

            foreach (var departamento in departamentos)
            {
                if (departamento.Id == 2)
                {
                    db.Entry(departamento).Collection(d => d.Funcionarios).Load();
                    /* Consulta explicita com condiçoes */
                    //db.Entry(departamento)
                    //    .Collection(d => d.Funcionarios)
                    //    .Query()
                    //    .Where(f => f.Id > 2)
                    //    .ToList(); // É necessario o uso do ToList()
                }

                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionario encontrado");
                }
            }
        }

        public static void CarregamentoLento()
        {
            using var db = new ApplicationDbContext();

            SetupTiposCarregamentos(db);

            /* desabilitando lazy load */
            //db.ChangeTracker.LazyLoadingEnabled = false;

            var departamentos = db.Departamentos;

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false) // Nesse momento os dados sao carregados
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionario encontrado");
                }
            }
        }
    }
}