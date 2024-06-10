using DominandoEFCore18.Data.Repositories;

namespace DominandoEFCore18.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartamentoRepository DepartamentoRepository { get; }

        IDepartamentoGenericRepository DepartamentoGenericRepository { get; }

        bool Commit();
    }
}