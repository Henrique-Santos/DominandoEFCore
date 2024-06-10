using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore08.Configurations
{
    public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            // Configurando um backing field
            //builder
            //    .Property(e => e.Cpf)
            //    .HasField("_cpf"); // Caso exista apenas uma propriedade publica de leitura

            builder
                .Property("_cpf") // Caso não exista um propriedade publica para leitura
                .HasColumnName("Cpf");
        }
    }
}