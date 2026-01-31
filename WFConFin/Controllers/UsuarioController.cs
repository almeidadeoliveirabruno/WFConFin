using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;
using WFConFin.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WFConFin.Controllers
{
    //3 usuarios Funcao
    //Gerente -> Pode realizar todas as operações
    //Empregado -> pode consultar, alterar e incluir novas informações, mas não pode excluir
    //Operador -> pode apenas consultar informações

    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //- faz com que todos os métodos precisem de autenticação.
    public class UsuarioController : ControllerBase
    {
        private readonly WFConfinDbContext _context;
        private readonly TokenService _tokenService;

        public UsuarioController(WFConfinDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [Route("Login")]
        [HttpPost]
        //[Authorize] //O usuário precisa estar autenticado para acessar este endpoint, independente do perfil
        [AllowAnonymous] //Permite que usuários não autenticados acessem este endpoint
        public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
        {
            var user = await _context.Usuario.Where(u => u.Login == usuarioLogin.Login ).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("Usuário Inválido");
            }
            if (user.Password != usuarioLogin.Password)
            {
                return Unauthorized("Senha Inválida");
            }

            var token = _tokenService.GerarToken(user);
            usuarioLogin.Password = ""; // para não passar a senha para o token

            // o ideal é passar os dados do usuário e do token como retorno
            var result = new
            {
                Usuario = usuarioLogin,
                Token = token
            };
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsuario()
        {
            try
            {
                var lista = await _context.Usuario.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na listagem de usuários. Exceção: {ex.Message}");
            }
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuarioController>
        [HttpPost]
        [Authorize(Roles = "Gerente, Empregado")] // Apenas usuários com a função "Gerente" podem acessar este endpoint. A role foi configurada em token service na parte de claims identity.
        public async Task<ActionResult> PostUsuario([FromBody] Usuario usuario)
        {
            try
            {
                var listUsuario = await _context.Usuario.Where(u => u.Login == usuario.Login).ToListAsync();
                if (listUsuario.Count > 0)
                    return BadRequest("Já existe um usuário com este login");
                await _context.Usuario.AddAsync(usuario); 
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível adicionar o usuário");
                return Ok("Usuário adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na exclusão do usuário. Exceção: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutUsuario([FromBody] Usuario usuario)
        {
            try
            {
                _context.Usuario.Update(usuario);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível atualizar o usuário");
                return Ok("Usuário atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na atualização do usuário. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")] // Apenas usuários com a função "Gerente" podem acessar este endpoint
        public async Task<ActionResult> Usuario([FromRoute] Guid id)
        {
            try
            {
                Usuario usuario = await _context.Usuario.FindAsync(id);
                if (usuario == null)
                    return BadRequest("Conta não encontrada");
                _context.Usuario.Remove(usuario);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível excluir o usuario");
                return Ok("Usuario excluído com sucesso");
                //12:21
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na alteração da conta. Código: {ex.Message}");
            }
        }



    }
}
