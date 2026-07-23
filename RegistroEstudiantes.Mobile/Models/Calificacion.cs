namespace RegistroEstudiantes.Mobile.Models;

public class Calificacion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Estudiante { get; set; } = string.Empty;

    public string Materia { get; set; } = string.Empty;

    public double Nota { get; set; }

    public string Observacion { get; set; } = string.Empty;

    public string Resumen => $"{Estudiante} - {Materia} - {Nota}";
}