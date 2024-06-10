using DominandoEFCore07.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore07
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /* ---------------- Infraestrutura ------------------------ */
            //ConsultarDepartamentos();
            //DadosSensiveis();
            //HabilitandoBatchSize();
            //TempoComandoGeral();
            //TempoComando();
            ExecutarEstrategiaResiliencia();
        }

        static void ExecutarEstrategiaResiliencia()
        {
            using var db = new ApplicationDbContext();

            var strategy = db.Database.CreateExecutionStrategy();

            // Ao usar transacoes de forma manual, a estrategia garante um roolback adequando quando houver varios erros devido a uma configuracao de retry

            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();

                db.Departamentos.Add(new Domain.Departamento { Descricao = "Departamento transacao"});

                db.SaveChanges();

                transaction.Commit();
            });
        }

        static void TempoComando()
        {
            using var db = new ApplicationDbContext();

            db.Database.SetCommandTimeout(10);

            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1");
        }

        static void TempoComandoGeral()
        {
            using var db = new ApplicationDbContext();

            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1");
        }

        static void HabilitandoBatchSize()
        {
            using var db = new ApplicationDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (int i = 0; i < 50; i++)
            {
                db.Departamentos.Add(new Domain.Departamento { Descricao = "Departamento " + i });
            }

           db.SaveChanges();
        }

        static void DadosSensiveis()
        {
            using var db = new ApplicationDbContext();

            var departamento = "Departamento";
            var departamentos = db.Departamentos.Where(d => d.Descricao == departamento).ToArray();
        }

        static void ConsultarDepartamentos()
        {
            using var db = new ApplicationDbContext();

            var departamentos = db.Departamentos.Where(d => d.Id > 0).ToArray();
        }
    }
}