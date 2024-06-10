using DominandoEFCore18.Data.Repositories.Base;
using DominandoEFCore18.Domain;

namespace DominandoEFCore18.Data.Repositories
{
    public interface IDepartamentoRepository
    {
        Task<Departamento> GetByIdAsync(int id);

        void Add(Departamento departamento);
    }
}