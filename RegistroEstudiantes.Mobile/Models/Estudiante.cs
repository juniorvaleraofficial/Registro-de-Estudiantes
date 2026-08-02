using SQLite;

namespace RegistroEstudiantes.Mobile.Models;

[Table("Estudiantes")]
public class Estudiante
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique]
    public string Matricula { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Carrera { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    [Ignore]
    public string NombreCompleto => $"{Nombre} {Apellido}";
}