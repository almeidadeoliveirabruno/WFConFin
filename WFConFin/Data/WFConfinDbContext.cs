using Microsoft.EntityFrameworkCore;
using WFConFin.Models;


namespace WFConFin.Data
{
    public class WFConfinDbContext : DbContext
    {
        public WFConfinDbContext(DbContextOptions<WFConfinDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Conta> Conta { get; set; }
        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Estado> Estado { get; set; }
    }
}
