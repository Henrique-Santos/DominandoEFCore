using DominandoEFCore13.Domain;
using DominandoEFCore13.Data;
using Microsoft.EntityFrameworkCore;
using DominandoEFCore13.Funcoes;

namespace DominandoEFCore13;

public class Program
{
    static void Main(string[] args)
    {
        /* ---------------- UDFs ------------------------ */
        //FuncaoLEFT();
        FuncaoDefinidaPeloUsuario();
    }

    static void DateDIFF()
    {
        CadastrarLivro();

        using var db = new ApplicationDbContext();

        /*
         * var resultado = db
         *  .Livros
         *  .Select(p=>  EF.Functions.DateDiffDay(p.CadastradoEm, DateTime.Now));
        */

        var resultado = db
            .Livros
            .Select(p => UserDefinedFunctions.DateDiff("DAY", p.CadastradoEm, DateTime.Now));

        foreach (var diff in resultado)
        {
            Console.WriteLine(diff);
        }
    }

    static void FuncaoDefinidaPeloUsuario()
    {
        CadastrarLivro();

        using var db = new ApplicationDbContext();

        db.Database.ExecuteSqlRaw(
            @"
                CREATE FUNCTION ConverterParaLetrasMaiusculas(@dados VARCHAR(100))
                RETURNS VARCHAR(100)
                BEGIN
                    RETURN UPPER(@dados)
                END
            "
        );


        var resultado = db.Livros.Select(p => UserDefinedFunctions.LetrasMaiusculas(p.Titulo));
        foreach (var parteTitulo in resultado)
        {
            Console.WriteLine(parteTitulo);
        }
    }

    static void FuncaoLEFT()
    {
        CadastrarLivro();

        using var db = new ApplicationDbContext();

        var resultado = db.Livros.Select(p => UserDefinedFunctions.Left(p.Titulo, 10));
        foreach (var parteTitulo in resultado)
        {
            Console.WriteLine(parteTitulo);
        }
    }

    static void CadastrarLivro()
    {
        using (var db = new ApplicationDbContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Livros.Add(
                new Livro
                {
                    Titulo = "Introdução ao Entity Framework Core",
                    Autor = "Rafael",
                    CadastradoEm = DateTime.Now.AddDays(-1)
                });

            db.SaveChanges();
        }
    }
}