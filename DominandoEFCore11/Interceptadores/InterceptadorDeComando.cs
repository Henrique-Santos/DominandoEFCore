using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace DominandoEFCore11.Interceptadores;

public partial class InterceptadorDeComando : DbCommandInterceptor
{
    private static readonly Regex _tableRegex = Regex();
    
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        Console.WriteLine("[Sync] Entrei dentro do método ReaderExecuting");

        UsarNoLock(command);

        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("[Async] Entrei dentro do método ReaderExecuting");

        UsarNoLock(command);

        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private static void UsarNoLock(DbCommand command)
    {
        if (!command.CommandText.Contains("WITH (NOLOCK)") && command.CommandText.StartsWith("-- Use NOLOCK"))
        {
            command.CommandText = _tableRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
        }
    }

    [GeneratedRegex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "pt-BR")]
    private static partial Regex Regex();
}