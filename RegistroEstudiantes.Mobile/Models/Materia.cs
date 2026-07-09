namespace RegistroEstudiantes.Mobile.Models;

public class Materia
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Profesor { get; set; } = string.Empty;

    public int Creditos { get; set; }

    public string DescripcionCorta => $"{Codigo} - {Nombre}";
}