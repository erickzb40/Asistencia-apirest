using System.ComponentModel.DataAnnotations;

namespace Asistencia_apirest.Entidades
{
    public class Empresa
    {
        [Key]
        public int id { get; set; }
        public int ruc { get; set; }
        public string razonsocial { get; set; }
        public string correo { get; set; }
    }
}
