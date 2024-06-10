using DominandoEFCore05a06.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DominandoEFCore05a06
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /* ---------------- Consultas ------------------------ */

            //FiltroGlobal();
            //IgnoreFiltroGlobal();
            //ConsultaProjetada();
            //ConsultaParametrizada();
            //ConsultaInterpolada();
            //ConsultaComTAG();
            //EntendendoConsulta_1N_N1();
            //DivisaoDeConsutas();

            /* ---------------- Procedures ------------------------ */

            //CriarStoredProcedure();
            //InserirDadosViaProcedure();
            //CriarStoredProcedureDeConsulta();
            //ConsultaViaProcedure();
        }

        public static void ConsultaViaProcedure()
        {
            using var db = new ApplicationDbContext();

            var departamentos = db.Departamentos.FromSqlRaw("EXECUTE GetDepartamentos @p0", "Departamento");

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        public static void CriarStoredProcedureDeConsulta()
        {
            var criarDepartamento =
                @"
                    CREATE OR ALTER PROCEDURE GetDepartamentos
                        @Descricao VARCHAR(50)
                    AS
                    BEGIN
                        SELECT * FROM Departamentos WHERE Descricao LIKE @Descricao + '%'
                    END
                ";

            using var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        public static void InserirDadosViaProcedure()
        {
            using var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw("EXECUTE CriarDepartamento @p0, @p1", "Departamento via procedure", true);
        }

        public static void CriarStoredProcedure()
        {
            var criarDepartamento = 
                @"
                    CREATE OR ALTER PROCEDURE CriarDepartamento
                        @Descricao VARCHAR(50),
                        @Ativo bit
                    AS
                    BEGIN
                        INSERT INTO Departamentos (Descricao, Ativo, Excluido)
                        VALUES (@Descricao, @Ativo, 0)
                    END
                ";

            using var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        public static void DivisaoDeConsutas()
        {
            using var db = new ApplicationDbContext();

            Setup(db);

            /* Evita esplosão cartesiana */
            var departamentos = db.Departamentos
                .Include(d => d.Funcionarios)
                .Where(d => d.Id < 3)
                .AsSplitQuery()
                //.AsSingleQuery() // Ignora o split query se estiver na configuracao global do ef-core
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario}");
                }
            }
        }

        public static void EntendendoConsulta_1N_N1()
        {
            using var db = new ApplicationDbContext();

            Setup(db);

            var departamentos = db.Departamentos
                .Include(d => d.Funcionarios) // 1N = LEFT JOIN - Mesmo que não exista funcionario ele vai trazer o departamento. Não força o relacionameto
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario.Nome}");
                }
            }

            var funcionarios = db.Funcionarios
                .Include(d => d.Departamento) // N1 = INNER JOIN - Força o relacionameto. Mostra apenas funcionario que possuem departamento. Isso pq funcionario é uma dependencia de departamento
                .ToList();

            foreach (var funcionario in funcionarios)
            {
                Console.WriteLine($"\t Nome: {funcionario.Nome} / Descrição: {funcionario.Departamento.Descricao}");
            }
        }

        public static void ConsultaComTAG()
        {
            using var db = new ApplicationDbContext();

            Setup(db);

            /* Cria um identificador na querie montada pelo ef-core. Pode servir para auditar comandos */
            var departamentos = db.Departamentos
                .TagWith(@"Estou enviando um comentário para o servidor
                    Mais um
                    Outro")
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        public static void ConsultaInterpolada()
        {
            using var db = new ApplicationDbContext();

            Setup(db);

            var id = 1;

            var departamentos = db.Departamentos
                .FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id > {id}")
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        public static void ConsultaParametrizada()
        {
            using var db = new ApplicationDbContext();

            Setup(db);

            /* O obj que o ef-core usa por debaixo dos panos ao lidar com parametros de consulta */
            var id = new SqlParameter { Value = 1, SqlDbType = SqlDbType.Int };

            var departamentos = db.Departamentos
                .FromSqlRaw("SELECT * FROM Departamentos WHERE Id > {0}", id)
                .Where(d => !d.Excluido)
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        public static void ConsultaProjetada()
        {
            using var db = new ApplicationDbContext();

            Setup(db);

            var departamentos = db.Departamentos
                .Where(d => d.Id > 0)
                .Select(d => new { d.Descricao, Funcionarios = d.Funcionarios.Select(f => f.Nome) })
                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario}");
                }
            }
        }

        public static void IgnoreFiltroGlobal()
        {
            using var db = new ApplicationDbContext();

            Setup(db);

            var departamentos = db.Departamentos.IgnoreQueryFilters().Where(d => d.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        public static void FiltroGlobal()
        {
            using var db = new ApplicationDbContext();

            Setup(db);
            
            var departamentos = db.Departamentos.Where(d => d.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }

        public static void Setup(ApplicationDbContext db)
        {
            if (db.Database.EnsureCreated()) // Cria o banco se ele não existir
            {
                db.Departamentos.AddRange(
                   new Domain.Departamento
                   {
                       Ativo = true,
                       Descricao = "Departamento 01",
                       Funcionarios =
                       [
                            new() {
                                Nome = "Rafael",
                                Cpf = "99999999911",
                                RG = "2100061"
                            }
                       ],
                       Excluido = true,
                   },
                   new Domain.Departamento
                   {
                       Ativo = true,
                       Descricao = "Departamento 02",
                       Funcionarios =
                       [
                            new() {
                                Nome = "Bruno",
                                Cpf = "99999999922",
                                RG = "2100062"
                            },
                            new() {
                                Nome = "Eduardo",
                                Cpf = "99999999933",
                                RG = "2100063"
                            }
                       ]
                   }
               );

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        } 
    }
}