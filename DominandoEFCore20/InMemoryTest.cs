using DominandoEFCore20.Data;
using DominandoEFCore20.Entities;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore20
{
    public class InMemoryTest
    {
        [Fact]
        public void Deve_inserir_um_departamento()
        {
            // Arrange
            var departamento = new Departamento 
            { 
                Descricao = "Tecnologia",
                DataCadastro = DateTime.Now
            };

            // Setup
            var context = CreateContext();
            context.Departamentos.Add(departamento);

            // Act
            var inseridos = context.SaveChanges();

            // Assert
            Assert.Equal(1, inseridos);
        }

        [Fact]
        public void Nao_implementado_funcoes_de_datas_para_o_provider_inmemory()
        {
            // Arrange
            var departamento = new Departamento
            {
                Descricao = "Tecnologia",
                DataCadastro = DateTime.Now
            };

            // Setup
            var context = CreateContext();
            context.Departamentos.Add(departamento);

            // Act
            var inseridos = context.SaveChanges();

            var action = () => context.Departamentos.FirstOrDefault(p => EF.Functions.DateDiffDay(DateTime.Now, p.DataCadastro) > 0);

            // Assert
            Assert.Throws<InvalidOperationException>(action);
        }

        private ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryTest")
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}