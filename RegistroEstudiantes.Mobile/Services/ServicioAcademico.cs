using RegistroEstudiantes.Mobile.Models;

namespace RegistroEstudiantes.Mobile.Services;

public static class ServicioAcademico
{
    private static readonly List<Estudiante> estudiantes = new();
    private static readonly List<Materia> materias = new();
    private static readonly List<Asistencia> asistencias = new();
    private static readonly List<Calificacion> calificaciones = new();

    public static IReadOnlyList<Estudiante> ObtenerEstudiantes()
    {
        return estudiantes;
    }

    public static void AgregarEstudiante(Estudiante estudiante)
    {
        estudiantes.Add(estudiante);
    }

    public static IReadOnlyList<Materia> ObtenerMaterias()
    {
        return materias;
    }

    public static void AgregarMateria(Materia materia)
    {
        materias.Add(materia);
    }

    public static IReadOnlyList<Asistencia> ObtenerAsistencias()
    {
        return asistencias;
    }

    public static void AgregarAsistencia(Asistencia asistencia)
    {
        asistencias.Add(asistencia);
    }

    public static IReadOnlyList<Calificacion> ObtenerCalificaciones()
    {
        return calificaciones;
    }

    public static void AgregarCalificacion(Calificacion calificacion)
    {
        calificaciones.Add(calificacion);
    }

    public static void CargarDatosDePrueba()
    {
        if (estudiantes.Count > 0 || materias.Count > 0)
        {
            return;
        }

        estudiantes.Add(new Estudiante
        {
            Matricula = "MT-2023-00518",
            Nombre = "Junior",
            Apellido = "Valera",
            Carrera = "Ingeniería de Software",
            Telefono = "809-000-0000"
        });

        estudiantes.Add(new Estudiante
        {
            Matricula = "SD-18-11014",
            Nombre = "Yisel",
            Apellido = "Santana",
            Carrera = "Ingeniería de Software",
            Telefono = "809-000-0001"
        });

        materias.Add(new Materia
        {
            Codigo = "INF-4316",
            Nombre = "Programación de Aplicaciones Móviles",
            Profesor = "Profesor de la asignatura",
            Creditos = 4
        });
    }
}