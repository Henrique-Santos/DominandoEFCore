﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace DominandoEFCore11.Interceptadores;

public class InterceptadorDeConexao : DbConnectionInterceptor
{
    public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        Console.WriteLine("Entrei no metodo ConnectionOpening");

        var connectionString = ((SqlConnection)connection).ConnectionString;

        Console.WriteLine(connectionString);

        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            //DataSource="IP Segundo Servidor",
            ApplicationName = "CursoEFCore"
        };

        connection.ConnectionString = connectionStringBuilder.ToString();

        Console.WriteLine(connectionStringBuilder.ToString());

        return result;
    }
}