using DominandoEFCore18.Data.Repositories.Base;
using DominandoEFCore18.Domain;

namespace DominandoEFCore18.Data.Repositories
{
    public class DepartamentoGenericRepository : GenericRepository<Departamento>, IDepartamentoGenericRepository
    {
        public DepartamentoGenericRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}