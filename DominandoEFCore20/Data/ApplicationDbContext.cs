using DominandoEFCore20.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore20.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Departamento> Departamentos { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
            
    }
}