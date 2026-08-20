using MariaEmMeuLar.Models;
using Microsoft.EntityFrameworkCore;

namespace MariaEmMeuLar.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Inscricao> Inscricoes { get; set; }
    }
}