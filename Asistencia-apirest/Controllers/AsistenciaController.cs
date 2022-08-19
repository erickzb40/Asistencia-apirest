
using Asistencia_apirest.Entidades;
using DemoAPI.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private IAsistenciaRepository _AsistenciaRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AsistenciaController(
            IAsistenciaRepository AsistenciaRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _AsistenciaRepository = AsistenciaRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [ActionName(nameof(GetAsistenciasAsync))]
        public IEnumerable<Asistencia> GetAsistenciasAsync()
        {
            return _AsistenciaRepository.GetAsistencias();
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
            string webRootPath = _webHostEnvironment.WebRootPath;
            var stamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            byte[] bytes = Convert.FromBase64String(Asistencia.imagen);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                image.Save(@""+webRootPath+"/img/" +stamp + ".jpg", ImageFormat.Jpeg);
            }
            
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