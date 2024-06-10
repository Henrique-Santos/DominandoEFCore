using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore13.Funcoes;

public static class UserDefinedFunctions
{
    public static void Registrar(ModelBuilder modelBuilder)
    {
        var funcoes = typeof(UserDefinedFunctions).GetMethods().Where(p => Attribute.IsDefined(p, typeof(DbFunctionAttribute)));

        foreach (var funcao in funcoes)
        {
            modelBuilder.HasDbFunction(funcao);
        }
    }

    [DbFunction(name: "LEFT", IsBuiltIn = true)]
    public static string Left(string dados, int quantidade)
    {
        throw new NotImplementedException();
    }

    public static int DateDiff(string identificador, DateTime dataInicial, DateTime dataFinal)
    {
        throw new NotImplementedException();
    }

    public static string LetrasMaiusculas(string dados)
    {
        throw new NotImplementedException();
    }
}