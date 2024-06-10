using DominandoEFCore18.Data;
using DominandoEFCore18.Data.Repositories.Base;
using DominandoEFCore18.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore18.Data.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Departamento> _dbSet;

        public DepartamentoRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Departamento>(); // Substitui o uso de _context.Departamentos para _dbSet
        }

        public void Add(Departamento departamento)
        {
            _dbSet.Add(departamento);
        }

        public async Task<Departamento> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Colaboradores)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}