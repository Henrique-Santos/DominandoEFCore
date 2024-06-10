using DominandoEFCore08.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore08.Configurations
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            //builder
            //    .ToTable("Pessoas")
            //    .HasDiscriminator<int>("TipoPessoa")
            //    .HasValue<Pessoa>(3) // Alterando o volor do Discriminator no tipo Pessoa
            //    .HasValue<Instrutor>(6)
            //    .HasValue<Aluno>(99);

            builder.ToTable("Pessoas");
        }
    }

    // Ter essa configuração distinta sinaliza o ef-core a usar o TPT. (TPT) Tabela Por Tipo
    public class InstrutorConfiguration : IEntityTypeConfiguration<Instrutor>
    {
        public void Configure(EntityTypeBuilder<Instrutor> builder)
        {
            builder.ToTable("Instrutores");
        }
    }

    public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Alunos");
        }
    }
}