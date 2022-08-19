
using Asistencia_apirest.Entidades;
using Asistencia_apirest.Modelos.Interfaces;
using DemoAPI.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioRepository _UsuarioRepository;

        public UsuarioController(IUsuarioRepository UsuarioRepository)
        {
            _UsuarioRepository = UsuarioRepository;
        }

        [HttpPost("login")]
        [ActionName(nameof(GetUsuariosAsync))]
        public IEnumerable<Usuario> GetUsuariosAsync(Usuario usuario)
        {
            return _UsuarioRepository.GetUsuarios(usuario).Where(res => res.nombreusuario == usuario.nombreusuario && res.contrasena == usuario.contrasena);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetUsuarioById))]
        public ActionResult<Usuario> GetUsuarioById(int id)
        {
            var UsuarioByID = _UsuarioRepository.GetUsuarioById(id);
            if (UsuarioByID == null)
            {
                return NotFound();
            }
            return UsuarioByID;
        }


        [HttpPut("{id}")]
        [ActionName(nameof(UpdateUsuario))]
        public async Task<ActionResult> UpdateUsuario(int id, Usuario Usuario)
        {
            if (id != Usuario.usuarioid)
            {
                return BadRequest();
            }

            await _UsuarioRepository.UpdateUsuarioAsync(Usuario);

            return NoContent();

        }

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteUsuario))]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var Usuario = _UsuarioRepository.GetUsuarioById(id);
            if (Usuario == null)
            {
                return NotFound();
            }

            await _UsuarioRepository.DeleteUsuarioAsync(Usuario);

            return NoContent();
        }
    
    }
}