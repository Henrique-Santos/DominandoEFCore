using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore08.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            // Usando fluent api para configurar o estado
            builder
                .HasOne(e => e.Governador)
                .WithOne(g => g.Estado)
                .HasForeignKey<Governador>(g => g.EstadoId);

            // Faz o Include automtica em uma consulta
            builder.Navigation(e => e.Governador).AutoInclude();

            builder
                .HasMany(e => e.Cidades)
                .WithOne(c => c.Estado)
                .IsRequired(false) // Te possibilita criar uma cidade sem existir um estado associado a ela. (Não recomendado)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}