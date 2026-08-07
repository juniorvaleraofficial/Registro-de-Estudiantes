using SQLite;

namespace RegistroEstudiantes.Mobile.Models;

[Table("Materias")]
public class Materia
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique]
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Profesor { get; set; } = string.Empty;

    public int Creditos { get; set; }

    [Ignore]
    public string DescripcionCorta => $"{Codigo} - {Nombre}";
}