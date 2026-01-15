using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFConFin.Models
{
    public class Pessoa
    {
        [Key]
        public Guid Id { get; set; } //guid garante o campo unico

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; }

        [StringLength(20, ErrorMessage = "O campo Telefone deve ter no máximo 20 caracteres")]
        public string Telefone { get; set; }

        [EmailAddress]
        public string Email { get; set; } // validação para email

        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; } // apenas data

        [Column(TypeName = "decimal(18,2)")]
        public Decimal Salario { get; set; } // monta do tipo decimal com 18 digitos e 2 casas decimais

        [StringLength(20,ErrorMessage = "O campo Gênero deve ter até 20 caracteres")]
        public string Genero { get; set; }

        public Pessoa()
        {
            Id = Guid.NewGuid();
        }

        public Guid? CidadeId { get; set; } //chave estrangeira para a entidade Cidade. Campo pode ser nulo
        public Cidade Cidade { get; set; } //relacionamento com a entidade Cidade

    }
}
