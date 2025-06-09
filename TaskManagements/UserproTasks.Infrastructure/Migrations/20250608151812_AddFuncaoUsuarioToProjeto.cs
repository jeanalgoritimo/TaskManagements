using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserProTasks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFuncaoUsuarioToProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeUsuario",
                table: "Tarefas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Tarefas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "FuncaoUsuario",
                table: "Projetos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeUsuario",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "FuncaoUsuario",
                table: "Projetos");
        }
    }
}
