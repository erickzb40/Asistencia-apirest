using Asistencia_apirest.Entidades;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asistencia_apirest.Controllers
{
    public class LocalController : Controller
    {
        protected readonly SampleContext _context;
        public LocalController(SampleContext context)=> _context = context;
        // GET: LocalController

        [HttpGet("local")]
        [ActionName(nameof(GetLocal))]
        public IEnumerable<Local> GetLocal()
        {
            return _context.Local.FromSqlRaw(@"select * from local").ToList();
        }

  
    }
}
