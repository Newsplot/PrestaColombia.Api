using PrestaColombia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PrestaColombia.Infrastructure.Data
{
    public class PrestaColombiaDbContext : DbContext
    {
        public PrestaColombiaDbContext(DbContextOptions<PrestaColombiaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
    }
}