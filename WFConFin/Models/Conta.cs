using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFConFin.Models
{
    public class Conta
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo deve ter entre 3 e 200 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo Data de Vencimento é obrigatório")]
        [DataType(DataType.Date)]
        public DateTime DataVencimento { get; set; } // apenas data

        [DataType(DataType.Date)]
        public DateTime? DataPagamento { get; set; } // apenas data

        [Required(ErrorMessage = "O campo Situação é obrigatório")]
        public Situacao Situacao { get; set; } // Enum para Situação - no banco salva como 0 ou 1

        public Conta()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid PessoaId { get; set; } // chave estrangeira para a entidade Pessoa.
        public Pessoa Pessoa { get; set; } // relacionamento com a entidade Pessoa
    }
}
