using DominandoEFCore08.Conversores;
using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore08.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Ator> Atores { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Instrutor> Instrutores { get; set; }
        // Property Bags
        public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "Data source=(localdb)\\mssqllocaldb; Initial Catalog=EFCore07;Integrated Security=true;MultipleActiveResultSets=true;";

            optionsBuilder
                .UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            // Collation global
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");
            // SQL_Latin1_General -> Designador de agrupamento
            // CI -> Indica que nao vai diferenciasr maiscula de minuscula. O contrario seria CS
            // AI -> Indica que vai ignorar acentuaçoes. O contrario seria AS

            // Collation especifica por propriedade
            modelBuilder.Entity<Departamento>().Property(p => p.Descricao).UseCollation("SQL_Latin1_General_CP1_CS_AS");

            // Criando uma sequencia customizada
            modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias").StartsAt(1).IncrementsBy(2);

            // Ao inves de ter um Id gerado pelo autoincrement, será usado a sequcncia customizada
            modelBuilder.Entity<Departamento>().Property(p => p.Id).HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");

            // Criado um indice
            modelBuilder.Entity<Departamento>()
                .HasIndex(d => new { d.Descricao, d.Ativo }) // Indice composto
                //.HasIndex(d => d.Descricao) // Indicie unico
                .HasDatabaseName("idx_meu_indice");

            // Criado um seed de dados
            modelBuilder.Entity<Estado>()
                .HasData(
                [
                    new Estado{ Id = 1, Nome = "Sao Paulo" },
                    new Estado{ Id = 2, Nome = "Sergipe" },
                ]);

            // Criando schemas
            modelBuilder.HasDefaultSchema("cadastros");

            modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoEsquema");

            // Convertendo campos
            modelBuilder.Entity<Conversor>()
                .Property(c => c.Versao)
                .HasConversion<string>(); // Salva no banco um tipo enum no formato varchar
                //.HasConversion(v => v.ToString(), v => (Versao)Enum.Parse(typeof(Versao), v)); // Salva no banco um tipo enum no formato varchar e ao retonar os dados converte por meio de um Parse a string para enum (isso é feito por padrao)

            // Usando conversor customizado
            modelBuilder.Entity<Conversor>()
                .Property(c => c.Status)
                .HasConversion(new ConversorCustomizado());

            // Criando uma propriedade de sombra
            modelBuilder.Entity<Departamento>().Property<DateTime>("UltimaAtualizacao");
            
            /* Informa ao ef-core que vc possui configurações do modelBuilder em arquivos separados. 
               Aplica as configurações de todas as classes que implementam IEntityTypeConfiguration
            */
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Configurando a Property Bags
            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", b =>
            {
                b.Property<int>("Id"); // O ef-core não consegue persistir dados em uma tabela que não possua PK

                b.Property<string>("Chave")
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();

                b.Property<string>("Valor")
                    .HasColumnType("VARCHAR(255)")
                    .IsRequired();
            });
        }
    }
}