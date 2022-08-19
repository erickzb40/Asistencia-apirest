using Asistencia_apirest.Entidades;
using System.Net.Mime;

namespace DemoAPI.Models.Repository
{
    public interface IAsistenciaRepository
    {
        Task<Asistencia> CreateAsistenciaAsync(Asistencia Asistencia);
        Task<bool> DeleteAsistenciaAsync(Asistencia Asistencia);
        Asistencia GetAsistenciaById(int id);
        IEnumerable<Asistencia> GetAsistencias();
        Task<bool> UpdateAsistenciaAsync(Asistencia Asistencia);
    }
}