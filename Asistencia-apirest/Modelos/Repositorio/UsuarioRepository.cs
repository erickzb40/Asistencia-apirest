
using Asistencia_apirest.Entidades;
using Asistencia_apirest.Modelos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Models.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        protected readonly SampleContext _context;
        public UsuarioRepository(SampleContext context) => _context = context;

        public IEnumerable<Usuario> GetUsuarios(Usuario usuario)
        {
            return _context.Usuario.ToList();
        }
        public IEnumerable<Usuario> demo()
        {
            return _context.Usuario.FromSqlRaw(@"select * from usuario").ToList();
        }

        public Usuario GetUsuarioById(int id)
        {
            return _context.Usuario.Find(id);
        }
        public async Task<Usuario> CreateUsuarioAsync(Usuario Usuario)
        {
            await _context.Set<Usuario>().AddAsync(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<bool> UpdateUsuarioAsync(Usuario Usuario)
        {
            _context.Entry(Usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUsuarioAsync(Usuario Usuario)
        {
            //var entity = await GetByIdAsync(id);
            if (Usuario is null)
            {
                return false;
            }
            _context.Set<Usuario>().Remove(Usuario);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}