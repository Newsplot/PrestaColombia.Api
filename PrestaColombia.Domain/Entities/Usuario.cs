using Prestacol.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestaColombia.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public Guid RolId { get; set; }

        public Rol Rol { get; set; } = null!;

        public bool Estado { get; set; } = true;
    }
}
