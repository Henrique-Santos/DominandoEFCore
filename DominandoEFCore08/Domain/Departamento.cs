﻿namespace DominandoEFCore08.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public List<Funcionario> Funcionarios { get; set; }
    }
}
