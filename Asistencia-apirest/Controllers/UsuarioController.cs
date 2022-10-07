
using Asistencia_apirest.Entidades;
using Asistencia_apirest.services;
using DemoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private SampleContext _context;
        private cifrado _cifrado;
        public UsuarioController(SampleContext context_,cifrado cifrado_)
        {
            _context = context_;
            _cifrado = cifrado_;
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetUsuariosAsync(Usuario usuario)
        {
            var query = await _context.Empresa.FirstOrDefaultAsync(res=>res.descripcion.Equals(usuario.empresa));
            if (query == null) {
                return NotFound("No se encontro la empresa");
            }
            if (query.cadenaconexion == null) { 
                return NotFound("No se encontro la empresa"); 
            }
            using (var context = new SampleContext(query.cadenaconexion)) {
                var result = await (from a in context.Usuario.Where(
                    res => res.nombreusuario.Equals(usuario.nombreusuario) && res.contrasena.Equals(usuario.contrasena))
                                    select a).ToListAsync();
                if (result==null)
                {
                    return NotFound("No se encontro ningun usuario");
                }
                var cifrado= _cifrado.EncryptStringAES(usuario.empresa+" "+usuario.nombreusuario+" "+usuario.contrasena);
                return Ok("{\"token\":\"" + cifrado + "\"}");
            }
        }

       
    
    }
}