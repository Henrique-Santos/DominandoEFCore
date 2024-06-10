namespace DominandoEFCore08.Domain
{
    public class Documento
    {
        private string _cpf;

        public int Id { get; set; }
        //public string Cpf => _cpf;

        public void SetCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) 
            { 
                throw new ArgumentNullException("CPF Invalido"); 
            }
            _cpf = cpf;
        }

        public string GetCpf() => _cpf;
    }
}