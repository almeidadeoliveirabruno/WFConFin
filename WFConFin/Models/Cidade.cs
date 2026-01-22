using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WFConFin.Models
{
    public class Cidade
    {
        [Key] // chave primária
        public Guid Id { get; set; } //guid garante o campo unico
        [Required(ErrorMessage = "O campo Nome é obrigatório")] // campo obrigatório com mensagem de erro
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo deve ter entre 3 e 200 caracteres")] // tamanho entre 3 e 100 caracteres com mensagem de erro
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "O campo Estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo deve ter 2 caracteres")]
        public string EstadoSigla { get; set; } // Nome da entidade + Campo

        public Cidade ()
        {
            Id = Guid.NewGuid(); // Gera um novo GUID ao criar uma instância de Cidade
        }

        [JsonIgnore] //Faz com que o campo não seja serializado em JSON/ ignorado no jason
        public Estado? Estado { get; set; } // Relacionamento com a entidade Estado
    }


}
