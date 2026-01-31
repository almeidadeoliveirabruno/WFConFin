using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : Controller
    {
        private readonly WFConfinDbContext _context;
        public ContaController(WFConfinDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetListaConta()
        {
            try
            {
                var lista = await _context.Conta.ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na listagem de contas. Exceção: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostConta([FromBody] Conta conta)
        {
            try
            {
                await _context.Conta.AddAsync(conta);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível adicionar a conta");
                return Ok("Conta adicionada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na exclusão da conta. Exceção: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutConta([FromBody] Conta conta)
        {
            try
            {
                _context.Conta.Update(conta);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível atualizar a conta");
                return Ok("Conta atualizada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na atualização da conta. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteConta([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _context.Conta.FindAsync(id);
                if (conta == null)
                    return BadRequest("Conta não encontrada");
                _context.Conta.Remove(conta);
                var resultado = await _context.SaveChangesAsync();
                if (resultado == 0)
                    return BadRequest("Não foi possível excluir a conta");
                return Ok("Conta excluída com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro na alteração da conta. Código: {ex.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetContaPesquisa([FromQuery] string valor)
        {
            try
            {
                var lista = await _context.Conta.Include(o=> o.Pessoa) // traz os dados de pessoa também
                    .Where(e => e.Descricao.ToUpper().Contains(valor.ToUpper()) || e.Pessoa.Nome.ToUpper().Contains(valor.ToUpper()))
                    .ToListAsync();
                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de Conta. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetContaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            //skip ignora info de registro de um sql
            //take numero de informações que quer trazer
            try
            {

                var lista = from o in _context.Conta.Include(o=> o.Pessoa)
                            where o.Descricao.ToUpper().Contains(valor.ToUpper()) || o.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                            select o; //traz todos os nomes

                if (ordemDesc)
                {
                    lista = from o in lista
                            orderby o.Descricao descending
                            select o;
                }
                else
                {
                    lista = from o in lista
                            orderby o.Descricao ascending
                            select o;
                }

                var qtde = lista.Count(); //quantidade total de registros da consulta
                var dados = await lista.Skip(skip).Take(take).ToListAsync(); //pular e trazer a quantidade
                var paginacaoResponse = new PaginacaoResponse<Conta>(dados, qtde, skip, take);
                return Ok(paginacaoResponse);


            }
            catch (Exception e)
            {
                return BadRequest("Erro, consulta de Pessoa. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

        [HttpGet("Pessoa/{pessoaid}")]
        public async Task<IActionResult> GetContasPessoa([FromRoute] Guid pessoaid)
        {
            //skip ignora info de registro de um sql
            //take numero de informações que quer trazer
            try
            {

                var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                            where o.PessoaId == pessoaid
                            select o; //traz todos os nomes

              return Ok(lista);

            }
            catch (Exception e)
            {
                return BadRequest("Erro na pesquisa de conta por pessoa. Exceção: " + e.Message); //mensagem caso de algum erro
            }
        }

    }
}
