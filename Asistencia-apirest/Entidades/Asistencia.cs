﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Asistencia_apirest.Entidades
{
    public class Asistencia
    {
        [Key]
        public int? id { get; set; }
        public DateTime? fecha { get; set; }
        public string? tipo { get; set; }    
        public int cod_empleado { get; set; }   
        public string? identificador { get; set; }
        public string? imagen { get; set; }
        public int? empresa { get; set; }
       
    }
}
