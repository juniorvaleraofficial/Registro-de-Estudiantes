using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Api.Models;

namespace RegistroEstudiantes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private static readonly List<StudentDto> Students =
    [
        new StudentDto
        {
            Id = 1,
            FirstName = "Junior",
            LastName = "Valera",
            StudentCode = "EST-001",
            Email = "junior.valera@email.com",
            Phone = "809-000-0001",
            Career = "Ingeniería de Software",
            BirthDate = new DateTime(2000, 8, 18),
            Status = "Activo"
        },
        new StudentDto
        {
            Id = 2,
            FirstName = "Yisel",
            LastName = "Santana",
            StudentCode = "EST-002",
            Email = "yisel.santana@email.com",
            Phone = "809-000-0002",
            Career = "Ingeniería de Software",
            BirthDate = new DateTime(1998, 1, 1),
            Status = "Activo"
        }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<StudentDto>> GetAll()
    {
        return Ok(Students);
    }

    [HttpGet("{id:int}")]
    public ActionResult<StudentDto> GetById(int id)
    {
        var student = Students.FirstOrDefault(student => student.Id == id);

        if (student is null)
        {
            return NotFound();
        }

        return Ok(student);
    }
}