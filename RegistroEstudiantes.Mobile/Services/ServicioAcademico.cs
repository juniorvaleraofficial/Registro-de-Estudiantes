using RegistroEstudiantes.Mobile.Models;

namespace RegistroEstudiantes.Mobile.Services;

public static class ServicioAcademico
{
    private static readonly List<Estudiante> estudiantes = new();
    private static readonly List<Materia> materias = new();
    private static readonly List<Asistencia> asistencias = new();
    private static readonly List<Calificacion> calificaciones = new();

    // =========================================================
    // ESTUDIANTES
    // =========================================================

    public static IReadOnlyList<Estudiante> ObtenerEstudiantes()
    {
        return estudiantes;
    }

    public static void AgregarEstudiante(Estudiante estudiante)
    {
        estudiantes.Add(estudiante);
    }

    public static bool ActualizarEstudiante(Estudiante estudianteActualizado)
    {
        var indice = estudiantes.FindIndex(
            estudiante => estudiante.Id == estudianteActualizado.Id);

        if (indice < 0)
        {
            return false;
        }

        estudiantes[indice] = estudianteActualizado;
        return true;
    }

    public static bool EliminarEstudiante(Guid id)
    {
        var estudiante = estudiantes.FirstOrDefault(
            estudiante => estudiante.Id == id);

        if (estudiante is null)
        {
            return false;
        }

        estudiantes.Remove(estudiante);
        return true;
    }

    // =========================================================
    // MATERIAS
    // =========================================================

    public static IReadOnlyList<Materia> ObtenerMaterias()
    {
        return materias;
    }

    public static void AgregarMateria(Materia materia)
    {
        materias.Add(materia);
    }

    public static bool ActualizarMateria(Materia materiaActualizada)
    {
        var indice = materias.FindIndex(
            materia => materia.Id == materiaActualizada.Id);

        if (indice < 0)
        {
            return false;
        }

        materias[indice] = materiaActualizada;
        return true;
    }

    public static bool EliminarMateria(Guid id)
    {
        var materia = materias.FirstOrDefault(
            materia => materia.Id == id);

        if (materia is null)
        {
            return false;
        }

        materias.Remove(materia);
        return true;
    }

    // =========================================================
    // ASISTENCIAS
    // =========================================================

    public static IReadOnlyList<Asistencia> ObtenerAsistencias()
    {
        return asistencias;
    }

    public static void AgregarAsistencia(Asistencia asistencia)
    {
        asistencias.Add(asistencia);
    }

    public static bool ActualizarAsistencia(Asistencia asistenciaActualizada)
    {
        var indice = asistencias.FindIndex(
            asistencia => asistencia.Id == asistenciaActualizada.Id);

        if (indice < 0)
        {
            return false;
        }

        asistencias[indice] = asistenciaActualizada;
        return true;
    }

    public static bool EliminarAsistencia(Guid id)
    {
        var asistencia = asistencias.FirstOrDefault(
            asistencia => asistencia.Id == id);

        if (asistencia is null)
        {
            return false;
        }

        asistencias.Remove(asistencia);
        return true;
    }

    // =========================================================
    // CALIFICACIONES
    // =========================================================

    public static IReadOnlyList<Calificacion> ObtenerCalificaciones()
    {
        return calificaciones;
    }

    public static void AgregarCalificacion(Calificacion calificacion)
    {
        calificaciones.Add(calificacion);
    }

    public static bool ActualizarCalificacion(
        Calificacion calificacionActualizada)
    {
        var indice = calificaciones.FindIndex(
            calificacion => calificacion.Id == calificacionActualizada.Id);

        if (indice < 0)
        {
            return false;
        }

        calificaciones[indice] = calificacionActualizada;
        return true;
    }

    public static bool EliminarCalificacion(Guid id)
    {
        var calificacion = calificaciones.FirstOrDefault(
            calificacion => calificacion.Id == id);

        if (calificacion is null)
        {
            return false;
        }

        calificaciones.Remove(calificacion);
        return true;
    }

    // =========================================================
    // DATOS INICIALES PARA PRUEBAS
    // =========================================================

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