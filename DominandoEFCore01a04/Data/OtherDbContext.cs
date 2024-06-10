using DominandoEFCore01a04.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore01a04.Data
{
    public class OtherDbContext : DbContext
    {
        public OtherDbContext() { }

        public OtherDbContext(DbContextOptions<OtherDbContext> options) : base(options) { }

        public DbSet<Other> Others { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore01a04;Integrated Security=true;MultipleActiveResultSets=true;";
            
            optionsBuilder
                .UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}