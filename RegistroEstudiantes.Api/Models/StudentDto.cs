namespace RegistroEstudiantes.Api.Models;

public class StudentDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string StudentCode { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Career { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; } = DateTime.Today;

    public string Status { get; set; } = "Activo";
}