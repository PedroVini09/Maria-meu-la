using MariaEmMeuLar.Models;
using Microsoft.EntityFrameworkCore;

namespace MariaEmMeuLar.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Missao> Missoes { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }
        public DbSet<Programacao> Programacoes { get; set; }
        public DbSet<Galeria> Galerias { get; set; }
        public DbSet<Contatos> Contatos { get; set; }

        public DbSet<Noticia> Noticias { get; set; }

        public DbSet<UsuarioAdmin> UsuariosAdmin { get; set; }

        public DbSet<MissaoHistoria> HistoriasMissoes { get; set; }

        public DbSet<MissaoSecao> MissoesSecoes { get; set; }

        public DbSet<MissaoSecaoItem> MissoesSecoesItens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais do modelo podem ser feitas aqui, se necessário

            modelBuilder.Entity<Missao>().ToTable("missao_tb");
            modelBuilder.Entity<Inscricao>().ToTable("inscricao_tb");
            modelBuilder.Entity<Programacao>().ToTable("programacao_tb");
            modelBuilder.Entity<Galeria>().ToTable("galeria_tb");
            modelBuilder.Entity<Contatos>().ToTable("contatos_tb");
            modelBuilder.Entity<Noticia>().ToTable("noticias_tb");
            modelBuilder.Entity<UsuarioAdmin>().ToTable("usuariosadmin_tb");
            modelBuilder.Entity<MissaoHistoria>(entity =>
            {
                entity.HasIndex(h => h.MissaoId).IsUnique();

                entity.HasOne(h => h.Missao).WithOne().HasForeignKey<MissaoHistoria>(h => h.MissaoId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.Property(h => h.DataAtualizacao).HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
            });
            modelBuilder.Entity<MissaoSecao>().HasOne(s => s.Missao).WithMany().HasForeignKey(s => s.MissaoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MissaoSecao>().HasIndex(s => new { s.MissaoId, s.Chave }).IsUnique();

            modelBuilder.Entity<MissaoSecaoItem>()
    .HasOne(i => i.Secao)
    .WithMany(s => s.Itens)
    .HasForeignKey(i => i.SecaoId)
    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}