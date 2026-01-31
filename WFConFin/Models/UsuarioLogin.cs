using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
    public class UsuarioLogin
    //classe para realizar o login do usuário apenas com login e senha
    {
        [Required(ErrorMessage = "O campo Login é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Login deve ter entre 3 e 200 caracteres")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo Password é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo Password deve ter entre 3 e 200 caracteres")]
        public string Password { get; set; }


    }
}
