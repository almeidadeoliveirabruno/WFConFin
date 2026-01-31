using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WFConFin.Models;

namespace WFConFin.Service
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var chave = Encoding.ASCII.GetBytes(_configuration.GetSection("Chave").Get<string>());  //busca a informação da chave em setting .json e converte em bytes

            var tokenDescritor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Login.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Funcao.ToString()),
                }
                ),
                Expires = DateTime.UtcNow.AddHours(2), //tempo de expiração do token,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature
                    ) // passa a chave e após a virgula é o algoritmo de criptografia
            };
            
            var token = tokenHandler.CreateToken(tokenDescritor); //criando o token

            return tokenHandler.WriteToken(token); // retornando o token em formato string com os dados geradows
        }
    }
}
