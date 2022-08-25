
using DemoAPI.Models;
using Empleado_apirest.Entidades;
using Empleado_apirest.Modelos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    { private SampleContext _context;
        private IEmpleadoRepository _EmpleadoRepository;

        public EmpleadoController(IEmpleadoRepository EmpleadoRepository,SampleContext context)
        {
            _EmpleadoRepository = EmpleadoRepository;
            _context = context;
        }

        [HttpGet]
        [ActionName(nameof(GetEmpleadosAsync))]
        public IEnumerable GetEmpleadosAsync()
        {
            var query = (from a in _context.Empleado
                         join sa in _context.Local on a.local equals sa.id
                         select new
                         {
                          a.id,
                          a.num_doc,
                          a.local,
                          a.codigo,
                          a.tipo_doc,
                          a.nombre,
                          sa.empresa,
                          sa.descripcion,
                          sa.ruc
                         }
                       ).ToList();
            return query;
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
        [HttpGet("codigoUpdate")]
        [ActionName(nameof(GetEmpleadoByCodigo))]
        public IEnumerable<Empleado> GetEmpleadoByCodigo(int codigo,int id)
        {
            return _EmpleadoRepository.GetEmpleadoByCodigo(codigo).Where(r=>r.id!=id);
        }
        [HttpGet("codigoInsert")]
        [ActionName(nameof(GetEmpleadoByCodigoInsert))]
        public IEnumerable<Empleado> GetEmpleadoByCodigoInsert(int codigo)
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