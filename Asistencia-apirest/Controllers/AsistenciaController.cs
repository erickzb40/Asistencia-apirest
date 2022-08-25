
using Asistencia_apirest.Entidades;
using DemoAPI.Models;
using DemoAPI.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private SampleContext _context;
        private IAsistenciaRepository _AsistenciaRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AsistenciaController(
            IAsistenciaRepository AsistenciaRepository,
            IWebHostEnvironment webHostEnvironment,
            SampleContext context)
        {
            _AsistenciaRepository = AsistenciaRepository;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpGet]
        [ActionName(nameof(GetAsistenciasAsync))]
        public IEnumerable GetAsistenciasAsync(int empresa)
        {
            var query = (from a in _context.Asistencia
                         join sa in _context.Empleado on a.cod_empleado equals sa.codigo
                         join local in _context.Local on sa.local equals local.id
                         join e in _context.Empresa on local.empresa equals e.id
                         where e.id == empresa orderby a.id descending
                         select new {
                             a.id,
                             a.cod_empleado,
                             a.fecha,
                             a.imagen,
                             a.identificador,
                             a.tipo,
                             sa.nombre,
                             sa.local,
                             sa.num_doc
                         }).Take(100).ToList();
            return query;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAsistenciaById))]
        public ActionResult<Asistencia> GetAsistenciaById(int id)
        {
            var AsistenciaByID = _AsistenciaRepository.GetAsistenciaById(id);
            if (AsistenciaByID == null)
            {
                return NotFound();
            }
            return AsistenciaByID;
        }

        [HttpPost]
        [ActionName(nameof(CreateAsistenciaAsync))]
        public async Task<ActionResult<Asistencia>> CreateAsistenciaAsync(Asistencia Asistencia)
        {
            await _AsistenciaRepository.CreateAsistenciaAsync(Asistencia);
            return CreatedAtAction(nameof(GetAsistenciaById), new { id = Asistencia.id }, Asistencia);
        }
    

        [HttpPut("{id}")]
        [ActionName(nameof(UpdateAsistencia))]
        public async Task<ActionResult> UpdateAsistencia(int id, Asistencia Asistencia)
        {
            if (id != Asistencia.id)
            {
                return BadRequest();
            }

            await _AsistenciaRepository.UpdateAsistenciaAsync(Asistencia);

            return NoContent();

        }

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteAsistencia))]
        public async Task<IActionResult> DeleteAsistencia(int id)
        {
            var Asistencia = _AsistenciaRepository.GetAsistenciaById(id);
            if (Asistencia == null)
            {
                return NotFound();
            }

            await _AsistenciaRepository.DeleteAsistenciaAsync(Asistencia);

            return NoContent();
        }
    }
}