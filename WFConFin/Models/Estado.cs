using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
    public class Estado
    {
        [Key] // chave primária
        [StringLength(2,MinimumLength=2,ErrorMessage ="O campo deve ter 2 caracteres")] // tamanho fixo de 2 caracteres, minimo 2 e mensagem de erro
        public string Sigla { get; set; }
        [Required(ErrorMessage ="O campo Nome é obrigatório")] // campo obrigatório com mensagem de erro
        [StringLength(60, MinimumLength =3,ErrorMessage ="O campo deve ter entre 3 e 200 caracteres")] // tamanho entre 3 e 200 caracteres com mensagem de erro
        public string Nome { get; set; }
    }
}
