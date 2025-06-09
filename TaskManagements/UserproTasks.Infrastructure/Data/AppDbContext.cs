using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace UserProTasks.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<HistoricoTarefa> HistoricoTarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             
            modelBuilder.Entity<Projeto>(entity =>
            {
                entity.HasKey(e => e.ProjetoId);
                entity.Property(e => e.ProjetoId).ValueGeneratedOnAdd();
                entity.Property(e => e.DataCriacao).HasColumnType("timestamp with time zone");

                
                entity.Property(e => e.FuncaoUsuario).HasMaxLength(50); 

                entity.HasMany(p => p.Tarefas)
                      .WithOne(t => t.Projeto)
                      .HasForeignKey(t => t.ProjetoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

           
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DataVencimento).HasColumnType("timestamp with time zone");

                entity.Property(t => t.Status)
                      .HasConversion<string>(); 

                entity.Property(t => t.Prioridade)
                      .HasConversion<string>(); 

                entity.HasMany(t => t.Comentarios)
                      .WithOne(c => c.Tarefa)
                      .HasForeignKey(c => c.TarefaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.Historico)
                      .WithOne(h => h.Tarefa)
                      .HasForeignKey(h => h.TarefaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DataCriacao).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<HistoricoTarefa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Data).HasColumnType("timestamp with time zone");
            });
        }
    }
}