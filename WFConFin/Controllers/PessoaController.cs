using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // url vai ter i final api/nomecontroller)]
    public class PessoaController : Controller
    {
        private readonly WFConfinDbContext _context;

        public PessoaController(WFConfinDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetListaPessoa()
        {
            try {
                var lista = await _context.Pessoa.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                await _context.Pessoa.AddAsync(pessoa);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível adicionar a pessoa");
                return Ok("Pessoa adicionada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na exclusão de pessoa. Exceção: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                _context.Pessoa.Update(pessoa);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível atualizar a pessoa");
                return Ok("Pessoa atualizada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na atualização da pessoa. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePessoa([FromRoute]Guid id)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);
                if (pessoa == null)
                    return BadRequest("Pessoa não encontrada");
                _context.Pessoa.Remove(pessoa);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível excluir a pessoa");
                return Ok("Pessoa excluída com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetPessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);
                if (pessoa == null)
                    return NotFound("Pessoa não encontrada");
                return Ok(pessoa);
     
            }
            catch (Exception ex)
            {
                return BadRequest($"Não possivel consultar a pessoa {ex.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetPessoaPesquisa([FromQuery] string valor)
        {
            try
            {

                //Método Entity
                var lista = await _context.Pessoa
                    .Where(e => e.Nome.ToUpper().Contains(valor.ToUpper()) || e.Telefone.ToUpper().Contains(valor.ToUpper()) || e.Email.ToUpper().Contains(valor.ToUpper()))
                    .ToListAsync();
                return Ok(lista);

                //Equivalente em SQL:
                // select * from estado where upper(sigla) like upper('%valor"%') OR upper(nome) like (%valor%)
            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de Pessoa. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetPessoaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            //skip ignora info de registro de um sql
            //take numero de informações que quer trazer
            try
            {

                var lista = from o in _context.Pessoa
                            where o.Nome.ToUpper().Contains(valor.ToUpper()) || o.Telefone.ToUpper().Contains(valor.ToUpper())|| o.Email.ToUpper().Contains(valor.ToUpper())
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
                var paginacaoResponse = new PaginacaoResponse<Pessoa>(dados, qtde, skip, take);
                return Ok(paginacaoResponse);


            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de Pessoa. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }



    }
}
