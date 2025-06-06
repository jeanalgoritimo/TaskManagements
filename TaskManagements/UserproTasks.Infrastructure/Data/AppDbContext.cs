
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

            // Mapeamento para Projeto
            modelBuilder.Entity<Projeto>(entity =>
            {
                entity.HasKey(e => e.ProjetoId);
                entity.Property(e => e.ProjetoId).ValueGeneratedOnAdd();
                entity.Property(e => e.DataCriacao).HasColumnType("timestamp with time zone");

                entity.HasMany(p => p.Tarefas)
                      .WithOne(t => t.Projeto) // Adicionado propriedade de navegação inversa em Tarefa para ser explícito
                      .HasForeignKey(t => t.ProjetoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Mapeamento para Tarefa
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DataVencimento).HasColumnType("timestamp with time zone");

                entity.Property(t => t.Status)
                      .HasConversion<string>(); // Armazenar como string

                entity.Property(t => t.Prioridade)
                      .HasConversion<string>(); // Armazenar como string

                // Relacionamento Um-Para-Muitos com Comentarios
                entity.HasMany(t => t.Comentarios)
                      .WithOne(c => c.Tarefa) // Adicionado propriedade de navegação inversa em Comentario
                      .HasForeignKey(c => c.TarefaId) // FK explícita
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacionamento Um-Para-Muitos com HistoricoTarefa
                entity.HasMany(t => t.Historico)
                      .WithOne(h => h.Tarefa) // Adicionado propriedade de navegação inversa em HistoricoTarefa
                      .HasForeignKey(h => h.TarefaId) // FK explícita
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Mapeamento para Comentario
            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.DataCriacao).HasColumnType("timestamp with time zone");

                // Relacionamento inverso com Tarefa (com propriedade de navegação)
                //entity.HasOne(c => c.Tarefa) // Se Comentario.Tarefa existir
                //      .WithMany(t => t.Comentarios)
                //      .HasForeignKey(c => c.TarefaId);
            });

            // Mapeamento para HistoricoTarefa
            modelBuilder.Entity<HistoricoTarefa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Data).HasColumnType("timestamp with time zone");

                // Relacionamento inverso com Tarefa (com propriedade de navegação)
                //entity.HasOne(h => h.Tarefa) // Se HistoricoTarefa.Tarefa existir
                //      .WithMany(t => t.Historico)
                //      .HasForeignKey(h => h.TarefaId);
            });
        }
    }
}