using DominandoEFCore18.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore18.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
    }
}