using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DominandoEFCore08.Conversores
{
    public class ConversorCustomizado : ValueConverter<Status, string>
    {
        public ConversorCustomizado() : base(s => ConverterParaOhBancoDeDados(s), v => ConverterParaAplicacao(v), new ConverterMappingHints(1))
        {
        }

        static string ConverterParaOhBancoDeDados(Status status)
        {
            return status.ToString()[0..1];
        }

        static Status ConverterParaAplicacao(string value)
        {
            var status = Enum.GetValues<Status>().FirstOrDefault(e => e.ToString()[0..1] ==  value);
            return status;
        }
    }
}