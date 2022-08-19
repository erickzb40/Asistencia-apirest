
using Empleado_apirest.Entidades;
using Empleado_apirest.Modelos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private IEmpleadoRepository _EmpleadoRepository;

        public EmpleadoController(IEmpleadoRepository EmpleadoRepository)
        {
            _EmpleadoRepository = EmpleadoRepository;
        }

        [HttpGet]
        [ActionName(nameof(GetEmpleadosAsync))]
        public IEnumerable<Empleado> GetEmpleadosAsync()
        {
            return _EmpleadoRepository.GetEmpleados();
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetEmpleadoById))]
        public ActionResult<Empleado> GetEmpleadoById(int id)
        {
            var EmpleadoByID = _EmpleadoRepository.GetEmpleadoById(id);
            if (EmpleadoByID == null)
            {
                return NotFound();
            }
            return EmpleadoByID;
        }
        [HttpGet("codigo")]
        [ActionName(nameof(GetEmpleadoByCodigo))]
        public IEnumerable<Empleado> GetEmpleadoByCodigo(int codigo)
        {
            return _EmpleadoRepository.GetEmpleadoByCodigo(codigo);
        }

        [HttpPost]
        [ActionName(nameof(CreateEmpleadoAsync))]
        public async Task<ActionResult<Empleado>> CreateEmpleadoAsync(Empleado Empleado)
        {
            await _EmpleadoRepository.CreateEmpleadoAsync(Empleado);
            return CreatedAtAction(nameof(GetEmpleadoById), new { id = Empleado.id }, Empleado);
        }

        [HttpPut("{id}")]
        [ActionName(nameof(UpdateEmpleado))]
        public async Task<ActionResult> UpdateEmpleado(int id, Empleado Empleado)
        {
            if (id != Empleado.id)
            {
                return BadRequest();
            }

            await _EmpleadoRepository.UpdateEmpleadoAsync(Empleado);

            return NoContent();

        }

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteEmpleado))]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var Empleado = _EmpleadoRepository.GetEmpleadoById(id);
            if (Empleado == null)
            {
                return NotFound();
            }

            await _EmpleadoRepository.DeleteEmpleadoAsync(Empleado);

            return NoContent();
        }

    }
}