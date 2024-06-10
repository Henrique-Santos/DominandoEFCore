using DominandoEFCore19.Data;
using DominandoEFCore19.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore19;

class Program
{
    static void Main(string[] args)
    {
        /* ---------------- Dicas e Truques ------------------------ */
        //ToQueryString();
        //DebugView();
        //Clear();
        //ConsultaFiltrada();
        //SingleOrDefaultVsFirstOrDefault();
        //SemChavePrimaria();
        //ToView();
        //NaoUnicode();
        //OperadoresDeAgregacao();
        //OperadoresDeAgregacaoNoAgrupamento();
        ContadorDeEventos();
    }

    static void ToQueryString()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureCreated();

        var query = db.Departamentos.Where(p => p.Id > 2);

        var sql = query.ToQueryString(); // Devolve a instrução sql gerada pelo ef-core.

        Console.WriteLine(sql);
    }

    static void DebugView()
    {
        using var db = new ApplicationDbContext();

        db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });
       
        var query = db.Departamentos.Where(p => p.Id > 2);
    }

    static void Clear()
    {
        using var db = new ApplicationDbContext();

        db.Departamentos.Add(new Departamento { Descricao = "TESTE DebugView" });

        db.ChangeTracker.Clear(); // Redefine o estado do contexto. Descarta os objetos que estão sendo rastreados
    }

    static void ConsultaFiltrada()
    {
        using var db = new ApplicationDbContext();

        var sql = db.Departamentos
            .Include(p => p.Colaboradores.Where(c => c.Nome.Contains("Teste")))
            .ToQueryString();

        Console.WriteLine(sql);
    }

    static void SingleOrDefaultVsFirstOrDefault()
    {
        using var db = new ApplicationDbContext();

        Console.WriteLine("SingleOrDefault:");

        _ = db.Departamentos.SingleOrDefault(p => p.Id > 2);

        Console.WriteLine("FirstOrDefault:");

        _ = db.Departamentos.FirstOrDefault(p => p.Id > 2);
    }

    static void SemChavePrimaria()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        // Em tabelas que não possuem PK o ef-core só é capaz de consultar os dados. Não podendo inserir, atualizar ou remover.
        var usuarioFuncoes = db.UsuarioFuncoes.Where(p => p.UsuarioId == Guid.NewGuid()).ToArray();
    }

    static void ToView()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Database.ExecuteSqlRaw(
            @"
                CREATE VIEW vw_departamento_relatorio 
                AS
                SELECT d.Descricao, COUNT(c.Id) as Colaboradores
                FROM Departamentos d 
                LEFT JOIN Colaboradores c ON c.DepartamentoId = d.Id
                GROUP BY d.Descricao
            "
        );

        var departamentos = Enumerable.Range(1, 10)
            .Select(p => new Departamento
            {
                Descricao = $"Departamento {p}",
                Colaboradores = Enumerable.Range(1, p)
                    .Select(c => new Colaborador { Nome = $"Colaborador {p}-{c}" })
                    .ToList()
            });

        var departamento = new Departamento
        {
            Descricao = $"Departamento Sem Colaborador"
        };

        db.Departamentos.Add(departamento);
        db.Departamentos.AddRange(departamentos);
        db.SaveChanges();

        var relatorio = db.DepartamentoRelatorio
            .Where(p => p.Colaboradores < 20)
            .OrderBy(p => p.Departamento)
            .ToList();

        foreach (var dep in relatorio)
        {
            Console.WriteLine($"{dep.Departamento} [ Colaboradores: {dep.Colaboradores}]");
        }
    }

    static void NaoUnicode()
    {
        using var db = new ApplicationDbContext();

        var sql = db.Database.GenerateCreateScript();

        Console.WriteLine(sql);
    }

    static void OperadoresDeAgregacao()
    {
        using var db = new ApplicationDbContext();

        var sql = db.Departamentos
            .GroupBy(p => p.Descricao)
            .Select(p =>
                new
                {
                    Descricao = p.Key,
                    Contador = p.Count(),
                    Media = p.Average(p => p.Id),
                    Maximo = p.Max(p => p.Id),
                    Soma = p.Sum(p => p.Id)
                }).ToQueryString();

        Console.WriteLine(sql);
    }

    static void OperadoresDeAgregacaoNoAgrupamento()
    {
        using var db = new ApplicationDbContext();

        var sql = db.Departamentos
            .GroupBy(p => p.Descricao)
            .Where(p => p.Count() > 1)
            .Select(p =>
                new
                {
                    Descricao = p.Key,
                    Contador = p.Count()
                }).ToQueryString();

        Console.WriteLine(sql);
    }

    static void ContadorDeEventos()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        Console.WriteLine($" PID: {Environment.ProcessId}");

        while (Console.ReadKey().Key != ConsoleKey.Escape)
        {
            var departamento = new Departamento
            {
                Descricao = $"Departamento Sem Colaborador"
            };

            db.Departamentos.Add(departamento);
            db.SaveChanges();

            _ = db.Departamentos.Find(1);
            _ = db.Departamentos.AsNoTracking().FirstOrDefault();
        }

    }
}