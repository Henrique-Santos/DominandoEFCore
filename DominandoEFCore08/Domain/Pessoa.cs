namespace DominandoEFCore08.Domain
{
    /* Com herança por padrão o ef-core criara apenas uma tabela Pessoa que tera todos os campos da classe e de seus filhos.
     * O ef-core diferenciara por um campo chamado Discriminator. O campo Discriminator geralmente tem o nome da classe.
     */
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

    public class Instrutor : Pessoa
    {
        public DateTime Desde { get; set; }
        public string Tecnologia { get; set; }
    }

    public class Aluno : Pessoa
    {
        public int Idade { get; set; }
        public DateTime DataContrato { get; set; }
    }
}