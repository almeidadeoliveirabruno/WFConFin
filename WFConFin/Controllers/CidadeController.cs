using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // url vai ter i final api/nomecontroller
    public class CidadeController : Controller
    {
        private readonly WFConfinDbContext _context;

        public CidadeController(WFConfinDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCidades()
        {
            try
            {
                // var result = _context.Cidade.Include(x=>x.Estado).ToList(); // Faz o join com a tabela Estado, mostrando os dados do estado junto com a cidade
                var result = _context.Cidade.ToList();
                return Ok(result);
            }

            catch(Exception e)
            {
                return BadRequest($"Erro na listagem de cidades. Exceção: {e.Message}");
            }
        }

        [HttpPost]
        public IActionResult PostCidade([FromBody] Cidade cidade)
        {
            try
            {
                _context.Cidade.Add(cidade);
                int valor = _context.SaveChanges(); // 0 se não conseguir salvar e 1 se salvou

                if (valor == 1)
                {
                    return Ok("A cidade foi adicionada");
                } else
                {
                    return BadRequest("Erro! A cidade não foi adicionada");
                }
                    
            }

            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de cidades. Exceção: {e.Message}");
            }
        }

        [HttpPut]
        public IActionResult PutCidade([FromBody] Cidade cidade)
        {
            try
            {
                _context.Cidade.Update(cidade);
                int valor = _context.SaveChanges(); // 0 se não conseguir salvar e 1 se salvou

                if (valor == 1)
                {
                    return Ok("A cidade foi alterada");
                }
                else
                {
                    return BadRequest("Erro! A cidade não foi alterada");
                }

            }

            catch (Exception e)
            {
                return BadRequest($"Erro na alteração de cidades. Exceção: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCidade([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = _context.Cidade.Find(id);
                if (cidade != null)
                {
                    _context.Cidade.Remove(cidade);
                    var valor = _context.SaveChanges();
                    if (valor == 1)
                    {
                        return Ok("Cidade removida com sucesso");
                    }
                    else
                    {
                        return BadRequest("Erro! A cidade não foi removida");
                    }
                }
                else
                {
                    return NotFound("Cidade não encontrada");
                }
            }

            catch (Exception e)
            {
                return BadRequest($"Erro na exclusão de cidade. Exceção: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCidade([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = _context.Cidade.Find(id);
                    if (cidade != null)
                    {
                        return Ok(cidade);
                    }
                    else
                    {
                        return NotFound("Erro! A cidade não existe");
                    }
            }

            catch (Exception e)
            {
                return BadRequest($"Erro na consulta de cidade. Exceção: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public IActionResult GetCidadePesquisa([FromQuery] string valor)
        {
            try
            {

                //Método Entity
                var lista = _context.Cidade
                    .Where(e => e.Nome.ToUpper().Contains(valor.ToUpper()) || e.EstadoSigla.ToUpper().Contains(valor.ToUpper()))
                    .ToList();
                return Ok(lista);

                //Equivalente em SQL:
                // select * from estado where upper(sigla) like upper('%valor"%') OR upper(nome) like (%valor%)
            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de cidade. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpGet("Paginacao")]
        public IActionResult GetCidadePaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            //skip ignora info de registro de um sql
            //take numero de informações que quer trazer
            try
            {

                var lista = from o in _context.Cidade
                            where o.EstadoSigla.ToUpper().Contains(valor.ToUpper()) || o.Nome.ToUpper().Contains(valor.ToUpper())
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
                var dados = lista.Skip(skip).Take(take).ToList(); //pular e trazer a quantidade
                var paginacaoResponse = new PaginacaoResponse<Cidade>(dados, qtde, skip, take);
                return Ok(paginacaoResponse);


            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de estado. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

    }
}
