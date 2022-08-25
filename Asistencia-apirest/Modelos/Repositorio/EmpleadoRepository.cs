

using Empleado_apirest.Entidades;
using Empleado_apirest.Modelos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Models.Repository
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        protected readonly SampleContext _context;
        protected readonly IWebHostEnvironment env;
        public EmpleadoRepository(SampleContext context, IWebHostEnvironment env) { 
            _context = context;
            this.env = env;
        }

        public async Task<Empleado> CreateEmpleadoAsync(Empleado empleado)
        {
            await _context.Set<Empleado>().AddAsync(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<bool> DeleteEmpleadoAsync(Empleado empleado)
        {
            //var entity = await GetByIdAsync(id);
            if (empleado is null)
            {
                return false;
            }
            _context.Set<Empleado>().Remove(empleado);
            await _context.SaveChangesAsync();

            return true;
        }

        public Empleado GetEmpleadoById(int id)
        {
            return _context.Empleado.Find(id);
        }

        public IEnumerable<Empleado> GetEmpleadoByCodigo(int codigo)
        {
            return _context.Empleado.ToList().Where(res=>res.codigo==codigo) ;
        }

        public IEnumerable<Empleado> GetEmpleados()
        {
            return _context.Empleado.FromSqlRaw(@"select empleado.id,nombre,num_doc,tipo_doc,local,local.descripcion,codigo 
            from EMPLEADO 
            inner join local on (empleado.local=local.id)
            where activo=1
            ").ToList();
        }

        public async Task GuardarArchivo(IFormFile archivo, string carpeta)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            string folder = Path.Combine(env.WebRootPath, carpeta);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string ruta = Path.Combine(folder, nombreArchivo);
            using (FileStream fileStream = File.Create(ruta))
            {
                await archivo.OpenReadStream().CopyToAsync(fileStream);
            }
        }

        public async Task<bool> UpdateEmpleadoAsync(Empleado empleado)
        {
            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}