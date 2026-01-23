using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]// url vai ter i final api/nomecontroller
    public class EstadoController : Controller
    {
        private readonly WFConfinDbContext _context;

        public EstadoController(WFConfinDbContext context) // VEM DA inicialização do projeto
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstados()
        {
            try
            {
                // Com o Task, o servidor libera o recurso para atender outras pessoas enquanto o banco de dados trabalha. Isso aumenta drasticamente a escalabilidade da  aplicação.

                var result = await _context.Estado.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Erro na listagem de estados. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEstados([FromBody]Estado estado)
        {
            try
            {
                await _context.Estado.AddAsync(estado);
                var valor = await _context.SaveChangesAsync(); // retorna 0 ou 1, se deu certo vai ser igual a 1
                if (valor == 1)
                {
                    return Ok("Sucesso, estado incluído "); //mensagem caso de algum erro
                } 
                else
                {
                    return BadRequest("Erro,estado não incluído. "); //mensagem caso de algum erro
                }
            }
            catch (Exception e)
            {
                return BadRequest("Erro de estado não incluído. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutEstados([FromBody] Estado estado)
        {
            try
            {
                _context.Estado.Update(estado);
                var valor = await _context.SaveChangesAsync(); // retorna 0 ou 1, se deu certo vai ser igual a 1
                if (valor == 1)
                {
                    return Ok("Sucesso, estado incluído "); //mensagem caso de algum erro
                }
                else
                {
                    return BadRequest("Erro,estado não alterado. "); //mensagem caso de algum erro
                }
            }
            catch (Exception e)
            {
                return BadRequest("Erro, estado não alterado. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpDelete("{sigla}")]
        public async Task<IActionResult> DeleteEstados([FromRoute] string sigla)
        {
            try
            {
                var estado = await _context.Estado.FindAsync(sigla); // localizar o estado pela sigla
                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    _context.Estado.Remove(estado);
                    var valor = await _context.SaveChangesAsync(); // retorna 0 ou 1, se deu certo vai ser igual a 1
                    if (valor == 1)
                    {
                        return Ok("Sucesso, estado excluído "); //mensagem caso de algum erro
                    }
                    else
                    {
                        return BadRequest("Erro,estado não alterado. "); //mensagem caso de algum erro
                    }
                }
                else
                {
                    return NotFound("Erro,estado não encontrado. "); //mensagem caso não encontre
                }
            }
            catch (Exception e)
            {
                return BadRequest("Erro, estado não excluído. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpGet("{sigla}")]
        public async Task<IActionResult> GetEstado([FromRoute] string sigla)
        {
            try
            {
                var estado = await _context.Estado.FindAsync(sigla); // localizar o estado pela sigla
                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    return Ok(estado); //retorna o estado encontrado
                }
                else
                {
                    return NotFound("Erro,estado não encontrado. "); //mensagem caso não encontre
                }
            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de estado. Exceção: " + e.Message); //mensagem caso de algum erro
            } 
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetEstadoPesquisa([FromQuery] string valor)
        {
            try
            {

                //Método Entity
                var lista = await _context.Estado
                    .Where(e => e.Sigla.ToUpper().Contains(valor.ToUpper()) || e.Nome.ToUpper().Contains(valor.ToUpper()))
                    .ToListAsync();
                return Ok(lista);

                //Query Syntax
                //var lista = from o in _context.Estado
                //            where o.Sigla.ToUpper().Contains(valor.ToUpper()) || o.Nome.ToUpper().Contains(valor.ToUpper())
                //            select o; //traz todos os nomes
                //return Ok(lista);

                //Expression
                //Expression<Func<Estado, bool>> expressao = o => true;
                //expressao = o => o.Sigla.ToUpper().Contains(valor.ToUpper())
                //            || o.Nome.ToUpper().Contains(valor.ToUpper());
                //var lista = _context.Estado.Where(expressao).ToList();
                //return Ok(lista);

                //Equivalente em SQL:
                // select * from estado where upper(sigla) like upper('%valor"%') OR upper(nome) like (%valor%)
            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de estado. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }


        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetEstadoPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            //skip ignora info de registro de um sql
            //take numero de informações que quer trazer
            try
            {

                var lista = from o in _context.Estado
                            where o.Sigla.ToUpper().Contains(valor.ToUpper()) || o.Nome.ToUpper().Contains(valor.ToUpper())
                            select o; //traz todos os nomes

                if (ordemDesc)
                {
                    lista = from o in lista
                            orderby o.Nome descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Nome ascending
                            select o;
                }

                var qtde = lista.Count(); //quantidade total de registros da consulta
                var dados = await lista.Skip(skip).Take(take).ToListAsync(); //pular e trazer a quantidade
                var paginacaoResponse = new PaginacaoResponse<Estado>(dados, qtde, skip, take);
                return Ok(paginacaoResponse);


            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de estado. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }


    }
}
