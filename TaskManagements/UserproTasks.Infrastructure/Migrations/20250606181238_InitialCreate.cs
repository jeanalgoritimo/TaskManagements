using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserProTasks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projeto",
                columns: table => new
                {
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeUsuario = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeto", x => x.ProjetoId);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Prioridade = table.Column<string>(type: "text", nullable: false),
                    ProjetoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarefas_Projeto_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projeto",
                        principalColumn: "ProjetoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Texto = table.Column<string>(type: "text", nullable: false),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TarefaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Tarefas_TarefaId",
                        column: x => x.TarefaId,
                        principalTable: "Tarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoTarefas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TarefaId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoTarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoTarefas_Tarefas_TarefaId",
                        column: x => x.TarefaId,
                        principalTable: "Tarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_TarefaId",
                table: "Comentarios",
                column: "TarefaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoTarefas_TarefaId",
                table: "HistoricoTarefas",
                column: "TarefaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_ProjetoId",
                table: "Tarefas",
                column: "ProjetoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "HistoricoTarefas");

            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.DropTable(
                name: "Projeto");
        }
    }
}
