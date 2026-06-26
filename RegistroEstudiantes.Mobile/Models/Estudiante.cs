namespace RegistroEstudiantes.Mobile.Models;

public class Estudiante
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Matricula { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Carrera { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string NombreCompleto => $"{Nombre} {Apellido}";
}