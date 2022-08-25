using Asistencia_apirest.Entidades;
using Empleado_apirest.Entidades;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Models
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {
        }
        public DbSet<Asistencia> Asistencia { get; set; }
        public DbSet<Empleado> Empleado { get;  set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Local> Local { get; set; }

    }
}