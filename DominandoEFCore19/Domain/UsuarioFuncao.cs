using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore19.Domain;

[Keyless]
public class UsuarioFuncao
{
    public Guid UsuarioId { get; set; }
    public Guid FuncaoId { get; set; }
}