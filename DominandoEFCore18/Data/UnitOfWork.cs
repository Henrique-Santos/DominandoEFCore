using DominandoEFCore18.Data.Repositories;

namespace DominandoEFCore18.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        /*
         * Como a aplicação pode ter varios repositorios, não é bom inicar a propriedade junto com a instancia da classe.
         * A instanciação só vai ocorrer quando for necessario (quando a propriedade for acessada)
         */
        private IDepartamentoRepository _departamentoRepository;
        public IDepartamentoRepository DepartamentoRepository
        {
            get => _departamentoRepository ??= new DepartamentoRepository(_context);
        }

        private IDepartamentoGenericRepository _departamentoGenericRepository;
        public IDepartamentoGenericRepository DepartamentoGenericRepository
        {
            get => _departamentoGenericRepository ??= new DepartamentoGenericRepository(_context);
        }

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}