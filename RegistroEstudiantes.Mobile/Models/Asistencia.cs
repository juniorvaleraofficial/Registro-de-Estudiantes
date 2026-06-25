namespace RegistroEstudiantes.Mobile.Models;

public class Asistencia
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Estudiante { get; set; } = string.Empty;

    public string Materia { get; set; } = string.Empty;

    public DateTime Fecha { get; set; } = DateTime.Today;

    public string Estado { get; set; } = string.Empty;

    public string Resumen => $"{Estudiante} - {Materia} - {Estado}";
}