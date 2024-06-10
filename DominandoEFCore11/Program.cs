using DominandoEFCore11.Data;
using DominandoEFCore11.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore11;

public class Program
{
    static void Main(string[] args)
    {
        /* ---------------- Interceptadores ------------------------ */
        //TesteInterceptacao();
        TesteInterceptacaoSaveChanges();
    }

    static void TesteInterceptacaoSaveChanges()
    {
        using var db = new ApplicationDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        db.Funcoes.Add(new Funcao
        {
            Descricao1 = "Teste"
        });

        db.SaveChanges();
    }

    static void TesteInterceptacao()
    {
        using var db = new ApplicationDbContext();

        var consulta = db
            .Funcoes
            .TagWith("Use NOLOCK")
            .FirstOrDefault();

        Console.WriteLine($"Consulta: {consulta?.Descricao1}");
    }
}