using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;

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
    }
}
