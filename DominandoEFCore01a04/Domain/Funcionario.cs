namespace DominandoEFCore01a04.Domain
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string RG { get; set; }
        public bool Excluido { get; set; }
        public int DepartamentoId { get; set; }
        /* Necessario uso do virtual para o funcionamento do lazy loading */
        public virtual Departamento Departamento { get; set; }
    }
}