
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
        public IEnumerable GetEmpleadosAsync(int empresa)
        {
            var query = (from a in _context.Empleado
                         join sa in _context.Local on a.local equals sa.id
                         join e in _context.Empresa on sa.empresa equals e.id
                         where e.id== empresa
                         select new
                         {
                          a.id,
                          a.num_doc,
                          a.local,
                          a.codigo,
                          a.tipo_doc,
                          a.nombre,
                          a.activo,
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
        public IEnumerable GetEmpleadoByCodigo(int codigo,int id, int empresa)
        {
            var query = (from e in _context.Empleado
                         join l in _context.Local on e.local equals l.id
                         join emp in _context.Empresa on l.empresa equals emp.id
                         where emp.id == empresa && e.codigo == codigo && e.id != id
                         select new
                         {
                             e.id,
                             e.nombre,
                             e.num_doc,
                             e.tipo_doc,
                             e.codigo,
                             e.local,
                             e.activo
                         }
                       ).ToList();
            return query;
            //return _EmpleadoRepository.GetEmpleadoByCodigo(codigo).Where(r=>r.id!=id);
        }
        [HttpGet("codigoInsert")]
        [ActionName(nameof(GetEmpleadoByCodigoInsert))]
        public IEnumerable GetEmpleadoByCodigoInsert(int codigo, int empresa)
        { var query = (from e in _context.Empleado 
                       join l in _context.Local on e.local equals l.id
                       join emp in _context.Empresa on l.empresa equals emp.id 
                       where emp.id==empresa && e.codigo==codigo
                       select new {
                        e.id,
                        e.nombre,
                        e.num_doc,
                        e.tipo_doc,
                        e.codigo,
                        e.local,
                        e.activo
                       }
                       ).ToList();
            return query;
        }

        [HttpPost]
        [ActionName(nameof(CreateEmpleadoAsync))]
        public async Task<ActionResult<Empleado>> CreateEmpleadoAsync(Empleado Empleado)
        {
            await _EmpleadoRepository.CreateEmpleadoAsync(Empleado);
            return CreatedAtAction(nameof(GetEmpleadoById), new { id = Empleado.id }, Empleado);
        }

        [HttpPost("{id}")]
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