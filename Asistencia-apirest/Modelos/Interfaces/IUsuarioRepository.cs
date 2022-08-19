using Asistencia_apirest.Entidades;

namespace Asistencia_apirest.Modelos.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> CreateUsuarioAsync(Usuario Usuario);
        Task<bool> DeleteUsuarioAsync(Usuario Usuario);
        Usuario GetUsuarioById(int id);
        IEnumerable<Usuario> GetUsuarios(Usuario usuario);
        Task<bool> UpdateUsuarioAsync(Usuario Usuario);
        public IEnumerable<Usuario> demo();
    }
}
