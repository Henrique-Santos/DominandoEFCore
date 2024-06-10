namespace DominandoEFCore01a04.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        /* Necessario uso do virtual para o funcionamento do lazy loading */
        public virtual List<Funcionario> Funcionarios { get; set; }
    }
}