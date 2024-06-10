namespace DominandoEFCore08.Domain
{
    public class Estado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Governador Governador { get; set; }
        // Propriedade de navegação unica. Pq Cidade não possui referencia a Estado. O ef-core sabe lidar com essa situação
        public ICollection<Cidade> Cidades { get; set; } = new List<Cidade>();
    }

    public class Governador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Partido { get; set; }
        public int Idade { get; set; }
        // Em um relacionamento 1x1. Essa propriedade indica ao ef-core uma dependencia. Pq um Estado pode existir mais um Governador não
        public int EstadoId { get; set; } 
        public Estado Estado { get; set; }
    }

    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // Add apenas para configurar o relacionamento com Fluent api
        public int EstadoId { get; set; }
        public Estado Estado { get; set; }
    }
}