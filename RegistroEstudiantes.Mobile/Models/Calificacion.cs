using SQLite;

namespace RegistroEstudiantes.Mobile.Models;

[Table("Calificaciones")]
public class Calificacion
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed("UX_Calificacion", 1, Unique = true)]
    public string Estudiante { get; set; } = string.Empty;

    [Indexed("UX_Calificacion", 2, Unique = true)]
    public string Materia { get; set; } = string.Empty;

    public double Nota { get; set; }

    public string Observacion { get; set; } = string.Empty;

    [Ignore]
    public string Resumen => $"{Estudiante} - {Materia}: {Nota:N1}";
}