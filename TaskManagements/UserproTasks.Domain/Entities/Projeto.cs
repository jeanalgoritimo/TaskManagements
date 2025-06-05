using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities
{
    [Table("Projeto")]
    public class Projeto
    {
        public Guid ProjetoId { get; private set; }          // Chave primária INT
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Guid UsuarioId { get; private set; }         // FK para o usuário
        public string NomeUsuario { get; private set; }     // Nome capturado na criação

        public List<Tarefa> Tarefas { get; private set; } = new();

        public Projeto(string nome, string descricao, Guid usuarioId, string nomeUsuario)
        {
            Nome = nome;
            Descricao = descricao;
            DataCriacao = DateTime.Now;
            UsuarioId = usuarioId;
            NomeUsuario = nomeUsuario;
        }

        public bool PodeSerRemovido() => Tarefas.All(t => t.Status == StatusTarefa.Concluida);
    }

}
