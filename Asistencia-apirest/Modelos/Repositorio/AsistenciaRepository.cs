using Asistencia_apirest.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DemoAPI.Models.Repository
{
    public class AsistenciaRepository : IAsistenciaRepository
    {
        protected readonly SampleContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public AsistenciaRepository(SampleContext context) => _context = context;

        public IEnumerable<Asistencia> GetAsistencias()
        {
            return _context.Asistencia.ToList();
        }

        public Asistencia GetAsistenciaById(int id)
        {
            return _context.Asistencia.Find(id);
        }
        public async Task<Asistencia> CreateAsistenciaAsync(Asistencia Asistencia)
        {
            await _context.Set<Asistencia>().AddAsync(Asistencia);
            await _context.SaveChangesAsync();
            return Asistencia;
        }

        public async Task<bool> UpdateAsistenciaAsync(Asistencia Asistencia)
        {
            _context.Entry(Asistencia).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsistenciaAsync(Asistencia Asistencia)
        {
            //var entity = await GetByIdAsync(id);
            if (Asistencia is null)
            {
                return false;
            }
            _context.Set<Asistencia>().Remove(Asistencia);
            await _context.SaveChangesAsync();

            return true;
        }


    

    }
}