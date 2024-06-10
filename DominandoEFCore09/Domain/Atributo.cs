using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DominandoEFCore09.Domain;

[Table("TabelaAtributos")]
[Index(nameof(Id), nameof(Descricao), IsUnique = true)] // Definindo um indice composto para a tabela
[Comment("Comentario da tabela Atributo")]
public class Atributo
{
    [Key]
    public int Id { get; set; }

    [Column("MinhaDescricao", TypeName = "VARCHAR(100)")]
    public string Descricao { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // O ef-core nao irá incluir essa prop nas instruçoes de Insert e Update. Apenas na de Read
    public string Nome { get; set; }

    [Required]
    [MaxLength(255)]
    public string Observacao { get; set;}
}

// O ef-core nao consegue resolver de forma automatica quando duas entidades possuem mais de um tipo de relacionamento
public class Aeroporto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    [NotMapped]
    public string Teste { get; set; }
    [InverseProperty("AeroportoDePartida")]
    public ICollection<Voo> VoosDePartida {  get; set; }
    [InverseProperty("AeroportoDeChegada")]
    public ICollection<Voo> VoosDeChegada { get; set; }
}

public class Voo
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public Aeroporto AeroportoDePartida { get; set; }
    public Aeroporto AeroportoDeChegada { get; set; }
}

[Keyless] // Indica que essa tabela nao possui chave primaria. Tabelas com esse atributo nao sao rastreadas pelo ef-core. Geralemnte usado em Views ou consultas projetadas.
public class RelatorioFinanceiro
{
    public string Descricao { get; set; }
    public decimal Total { get; set; }
    public DateTime Data { get; set; }
}