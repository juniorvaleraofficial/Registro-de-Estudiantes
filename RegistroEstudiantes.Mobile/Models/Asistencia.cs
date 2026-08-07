using SQLite;

namespace RegistroEstudiantes.Mobile.Models;

[Table("Asistencias")]
public class Asistencia
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed("UX_Asistencia", 1, Unique = true)]
    public string Estudiante { get; set; } = string.Empty;

    [Indexed("UX_Asistencia", 2, Unique = true)]
    public string Materia { get; set; } = string.Empty;

    [Indexed("UX_Asistencia", 3, Unique = true)]
    public DateTime Fecha { get; set; } = DateTime.Today;

    public string Estado { get; set; } = string.Empty;

    [Ignore]
    public string Resumen => $"{Estudiante} - {Materia} - {Estado}";
}