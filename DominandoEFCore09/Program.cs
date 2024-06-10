using DominandoEFCore09.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore09;

public class Program
{
    static void Main(string[] args)
    {
        /* ---------------- Atributos - Data Annotations ------------------------ */
        Atributos();
    }

    static void Atributos()
    {
        using var db = new ApplicationDbContext();

        var script = db.Database.GenerateCreateScript();

        Console.WriteLine(script);
    }
}