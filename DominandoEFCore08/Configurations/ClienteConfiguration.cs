using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore08.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            // Cria todos os campo da classe Endereco na tabela de Cliente
            builder.OwnsOne(c => c.Endereco, o =>
            {
                o.Property(e => e.Bairro).HasColumnName("Bairro");

                // Fazendo Table Split pelo df-core. (Já nao add os campos de enderco na tabela de clientes)
                o.ToTable("Endereco");
            });
        }
    }
}