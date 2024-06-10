using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DominandoEFCore21a22;

public partial class MyInterceptor : IObserver<KeyValuePair<string, object>>
{
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(KeyValuePair<string, object> value)
    {
        if (value.Key == RelationalEventId.CommandExecuting.Name)
        {
            var command = ((CommandEventData)value.Value).Command;

            if (!command.CommandText.Contains("WITH (NOLOCK)"))
            {
                command.CommandText = _tableAliasRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");

                Console.WriteLine(command.CommandText);
            }
        }
    }

    private static readonly Regex _tableAliasRegex = Exec();

    [GeneratedRegex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WITH \(NOLOCK\)))", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled, "pt-BR")]
    private static partial Regex Exec();
}

public class MyInterceptorListener : IObserver<DiagnosticListener>
{
    private readonly MyInterceptor _interceptor = new();

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(DiagnosticListener value)
    {
        if (value.Name == DbLoggerCategory.Name)
        {
            value.Subscribe(_interceptor);
        }
    }
}