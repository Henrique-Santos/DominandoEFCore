using DominandoEFCore17.Domain;
using DominandoEFCore17.Provider;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore17.Data
{
    // ESTRATEGIA COM: IDENTIFICADOR NA TABELA
    /*
    public class ApplicationDbContext : DbContext
    {
        private readonly TenantData _tenant;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, TenantData tenant) : base(options)
        {
            _tenant = tenant;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = $"Person 1", TenantId = "tenant-1" },
                new Person { Id = 2, Name = $"Person 2", TenantId = "tenant-2" },
                new Person { Id = 3, Name = $"Person 3", TenantId = "tenant-2" });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Description = $"Description 1", TenantId = "tenant-1" },
                new Product { Id = 2, Description = $"Description 2", TenantId = "tenant-2" },
                new Product { Id = 3, Description = $"Description 3", TenantId = "tenant-2" });

            modelBuilder.Entity<Person>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);

            modelBuilder.Entity<Person>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);
        }
    }
    */

    // ESTRATEGIA COM: SCHEMA
    /*
    public class ApplicationDbContext : DbContext
    {
        public readonly TenantData TenantData;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, TenantData tenant) : base(options)
        {
            TenantData = tenant;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(TenantData.TenantId);

            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = $"Person 1", TenantId = "tenant-1" },
                new Person { Id = 2, Name = $"Person 2", TenantId = "tenant-2" },
                new Person { Id = 3, Name = $"Person 3", TenantId = "tenant-2" });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Description = $"Description 1", TenantId = "tenant-1" },
                new Product { Id = 2, Description = $"Description 2", TenantId = "tenant-2" },
                new Product { Id = 3, Description = $"Description 3", TenantId = "tenant-2" });
        }
    }
    */

    // ESTRATEGIA COM: BANCO DE DADOS
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = $"Person 1", TenantId = "tenant-1" },
                new Person { Id = 2, Name = $"Person 2", TenantId = "tenant-2" },
                new Person { Id = 3, Name = $"Person 3", TenantId = "tenant-2" });

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Description = $"Description 1", TenantId = "tenant-1" },
                new Product { Id = 2, Description = $"Description 2", TenantId = "tenant-2" },
                new Product { Id = 3, Description = $"Description 3", TenantId = "tenant-2" });
        }
    }
}