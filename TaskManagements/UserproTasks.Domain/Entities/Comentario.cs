﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class Comentario
    {
        public Guid Id { get; private set; }
        public string Texto { get; private set; }
        public string Usuario { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Comentario(string texto, string usuario)
        {
            Id = Guid.NewGuid();
            Texto = texto;
            Usuario = usuario;
            DataCriacao = DateTime.UtcNow;
        }
    }
}
