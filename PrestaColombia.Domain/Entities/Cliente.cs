using System;

namespace PrestaColombia.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Documento { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}