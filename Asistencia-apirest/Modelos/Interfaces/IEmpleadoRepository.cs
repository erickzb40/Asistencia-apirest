
using Empleado_apirest.Entidades;

namespace Empleado_apirest.Modelos.Interfaces
{
    public interface IEmpleadoRepository
    {
        Task<Empleado> CreateEmpleadoAsync(Empleado empleado);
        Task<bool> DeleteEmpleadoAsync(Empleado Empleado);
        Empleado GetEmpleadoById(int id);
        IEnumerable<Empleado> GetEmpleados();
        Task<bool> UpdateEmpleadoAsync(Empleado empleado);
        Task GuardarArchivo(IFormFile archivo,string carpeta);
        public IEnumerable<Empleado> GetEmpleadoByCodigo(int codigo);
    }
}
