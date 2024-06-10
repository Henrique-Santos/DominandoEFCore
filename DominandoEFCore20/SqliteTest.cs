using DominandoEFCore20.Data;
using DominandoEFCore20.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore20
{
    public class SqliteTest
    {
        [Theory]
        [InlineData("Tecnologia")]
        [InlineData("Financeiro")]
        [InlineData("Pessoas")]
        public void Deve_inserir_um_departamento(string descricao)
        {
            // Arrange
            var departamento = new Departamento 
            { 
                Descricao = descricao,
                DataCadastro = DateTime.Now
            };

            // Setup
            var context = CreateContext();
            context.Database.EnsureCreated();
            context.Departamentos.Add(departamento);

            // Act
            var inseridos = context.SaveChanges();
            departamento = context.Departamentos.FirstOrDefault(p => p.Descricao == descricao);

            // Assert
            Assert.Equal(1, inseridos);
            Assert.Equal(descricao, departamento.Descricao);
        }

        private ApplicationDbContext CreateContext()
        {
            var conection = new SqliteConnection("Datasource=:memory:");
            conection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(conection)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}