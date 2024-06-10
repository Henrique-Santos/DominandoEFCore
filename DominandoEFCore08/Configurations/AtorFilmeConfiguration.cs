using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore08.Configurations
{
    public class AtorFilmeConfiguration : IEntityTypeConfiguration<Ator>
    {
        public void Configure(EntityTypeBuilder<Ator> builder)
        {
            // Essa configuração é feita automtica pelo ef-core
            //builder
            //    .HasMany(a => a.Filmes)
            //    .WithMany(f => f.Atores)
            //    .UsingEntity(p => p.ToTable("AtoresFilmes")); // Renomeando tabela de junção

            builder
                .HasMany(a => a.Filmes)
                .WithMany(f => f.Atores)
                .UsingEntity<Dictionary<string, object>>(
                    "AtoresFilmes", // Renomeando tabela de junção
                    p => p.HasOne<Filme>().WithMany().HasForeignKey("FilmeId"), // Renomeando campo de Id do Filme
                    p => p.HasOne<Ator>().WithMany().HasForeignKey("AtorId"),
                    p =>
                    {
                        p.Property<DateTime>("CadastradoEm").HasDefaultValueSql("GETDATE()"); // Criado um shadow property
                    }
                );
        }
    }
}