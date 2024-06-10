using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore09.Domain;

public class Documento
{
    private string _cpf;

    public int Id { get; set; }

    [BackingField(nameof(_cpf))]
    public string Cpf => _cpf;

    public void SetCpf(string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
        {
            throw new ArgumentNullException("CPF Invalido");
        }
        _cpf = cpf;
    }

    public string GetCpf() => _cpf;
}