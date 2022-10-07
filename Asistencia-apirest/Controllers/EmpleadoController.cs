

using Asistencia_apirest.services;
using DemoAPI.Models;
using Empleado_apirest.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly SampleContext _context;
        private cifrado _cifrado;
        private util _util;

        public EmpleadoController(SampleContext context_, cifrado cifrado_, util util_)
        {
            _context = context_;
            _cifrado = cifrado_;
            _util = util_;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpleadosAsync(string token)
        {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null)
            {
                return NotFound("El token no es valido!");
            }
            var empresa = await _context.Empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0]);
            if (empresa == null)
            {
                return NotFound("La empresa ingresada no es válida.");
            }
            if (empresa.cadenaconexion == null)
            {
                return NotFound("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return NotFound("El usuario ingresado no es valido");
                }
                var usuario_locales = await context.Usuario_local.Where(res => res.usuarioid.Equals(usuario.usuarioid)).ToListAsync();
                if (usuario_locales == null)
                {
                    return NotFound("No hay locales asignados");
                }
                int[] locales = _util.convertirArray(usuario_locales);
                var query = await (from a in context.Empleado
                                   join sa in context.Local on a.local equals sa.id
                                   where locales.Contains(sa.id)
                                   select new
                                   {
                                       a.id,
                                       a.num_doc,
                                       a.local,
                                       a.codigo,
                                       a.tipo_doc,
                                       a.nombre,
                                       a.activo,
                                       sa.descripcion,
                                       sa.ruc
                                   }
                   ).ToListAsync();
                return Ok(query);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateEmpleado(string token, Empleado empleado)
        {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null)
            {
                return NotFound("El token no es valido!");
            }
            var empresa = await _context.Empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0]);
            if (empresa == null)
            {
                return NotFound("La empresa ingresada no es válida.");
            }
            if (empresa.cadenaconexion == null)
            {
                return NotFound("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return NotFound("El usuario ingresado no es valido");
                }
                var usuario_locales = await context.Usuario_local.Where(res => res.usuarioid.Equals(usuario.usuarioid)).ToListAsync();
                if (usuario_locales == null)
                {
                    return NotFound("No hay locales asignados");
                }
                var result = await context.Empleado.FirstOrDefaultAsync(b => b.id == empleado.id);
                var rep =    await context.Empleado.FirstOrDefaultAsync(res => res.codigo.Equals(empleado.codigo)&&res.id!=empleado.id);
                if (rep != null)
                {
                    return Problem("El codigo ingresado no esta disponible");
                }
                if (result != null)
                {
                    result.local = empleado.local;
                    result.codigo = empleado.codigo;
                    result.activo = empleado.activo;
                    result.nombre = empleado.nombre;
                    result.num_doc = empleado.num_doc;
                    result.tipo_doc = empleado.tipo_doc;
                    context.SaveChanges();
                    return Ok();
                }
                return NotFound("No se encontro el usuario");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertEmpleado(string token, Empleado empleado)
        {
            var vtoken = _cifrado.validarToken(token);
            if (vtoken == null)
            {
                return NotFound("El token no es valido!");
            }
            var empresa = await _context.Empresa.FirstOrDefaultAsync(x => x.descripcion == vtoken[0]);
            if (empresa == null)
            {
                return NotFound("La empresa ingresada no es válida.");
            }
            if (empresa.cadenaconexion == null)
            {
                return NotFound("La empresa ingresada no es válida.");
            }
            using (var context = new SampleContext(empresa.cadenaconexion))
            {
                var usuario = await context.Usuario.FirstOrDefaultAsync(res => res.nombreusuario.Equals(vtoken[1]) && res.contrasena.Equals(vtoken[2]));
                if (usuario == null)
                {
                    return NotFound("El usuario ingresado no es valido");
                }
                var rep = await context.Empleado.FirstOrDefaultAsync(res => res.codigo.Equals(empleado.codigo));
                if (rep==null)
                {
                    await context.Set<Empleado>().AddAsync(empleado);
                    await context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetEmpleadosAsync), new { id = empleado.id }, empleado);
                }
                return Problem("El codigo ingresado ya está en uso!");
            }
        }
    }
}