using Asistencia_apirest.Entidades;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Asistencia_apirest.Controllers
{
    public class LocalController : Controller
    {
        public readonly SampleContext _context;

        public LocalController(SampleContext context)=> _context = context;
        // GET: LocalController

        [HttpGet("local")]
        [ActionName(nameof(GetLocal))]
        public IEnumerable GetLocal(int empresa)
        { var query = (from a in _context.Local
                       where a.empresa == empresa
                       select new { a.id, a.descripcion, a.ruc }
                       ).ToList();
            return query;

        }

  
    }
}
