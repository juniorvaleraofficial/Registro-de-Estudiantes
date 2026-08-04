using Microsoft.Maui.Storage;
using RegistroEstudiantes.Mobile.Models;
using SQLite;

namespace RegistroEstudiantes.Mobile.Services;

public static class ServicioAcademico
{
    private static SQLiteAsyncConnection? baseDeDatos;
    private static bool inicializada;

    // =========================================================
    // INICIALIZACIÓN DE SQLITE
    // =========================================================

    private static async Task InicializarAsync()
    {
        if (inicializada)
        {
            return;
        }

        var rutaBaseDeDatos = Path.Combine(
            FileSystem.AppDataDirectory,
            "registro_estudiantes.db3");

        baseDeDatos = new SQLiteAsyncConnection(rutaBaseDeDatos);

        await baseDeDatos.CreateTableAsync<Estudiante>();
        await baseDeDatos.CreateTableAsync<Materia>();
        await baseDeDatos.CreateTableAsync<Asistencia>();
        await baseDeDatos.CreateTableAsync<Calificacion>();

        inicializada = true;
    }

    private static SQLiteAsyncConnection ObtenerConexion()
    {
        return baseDeDatos
            ?? throw new InvalidOperationException(
                "La base de datos todavía no ha sido inicializada.");
    }

    // =========================================================
    // ESTUDIANTES
    // =========================================================

    public static async Task<List<Estudiante>> ObtenerEstudiantesAsync()
    {
        await InicializarAsync();

        return await ObtenerConexion()
            .Table<Estudiante>()
            .OrderBy(estudiante => estudiante.Nombre)
            .ThenBy(estudiante => estudiante.Apellido)
            .ToListAsync();
    }

    public static async Task<int> AgregarEstudianteAsync(
        Estudiante estudiante)
    {
        await InicializarAsync();

        return await ObtenerConexion().InsertAsync(estudiante);
    }

    public static async Task<bool> ActualizarEstudianteAsync(
        Estudiante estudianteActualizado)
    {
        await InicializarAsync();

        var filasActualizadas = await ObtenerConexion()
            .UpdateAsync(estudianteActualizado);

        return filasActualizadas > 0;
    }

    public static async Task<bool> EliminarEstudianteAsync(int id)
    {
        await InicializarAsync();

        var filasEliminadas = await ObtenerConexion()
            .DeleteAsync<Estudiante>(id);

        return filasEliminadas > 0;
    }

    // =========================================================
    // MATERIAS
    // =========================================================

    public static async Task<List<Materia>> ObtenerMateriasAsync()
    {
        await InicializarAsync();

        return await ObtenerConexion()
            .Table<Materia>()
            .OrderBy(materia => materia.Nombre)
            .ToListAsync();
    }

    public static async Task<int> AgregarMateriaAsync(Materia materia)
    {
        await InicializarAsync();

        return await ObtenerConexion().InsertAsync(materia);
    }

    public static async Task<bool> ActualizarMateriaAsync(
        Materia materiaActualizada)
    {
        await InicializarAsync();

        var filasActualizadas = await ObtenerConexion()
            .UpdateAsync(materiaActualizada);

        return filasActualizadas > 0;
    }

    public static async Task<bool> EliminarMateriaAsync(int id)
    {
        await InicializarAsync();

        var filasEliminadas = await ObtenerConexion()
            .DeleteAsync<Materia>(id);

        return filasEliminadas > 0;
    }

    // =========================================================
    // ASISTENCIAS
    // =========================================================

    public static async Task<List<Asistencia>> ObtenerAsistenciasAsync()
    {
        await InicializarAsync();

        return await ObtenerConexion()
            .Table<Asistencia>()
            .OrderByDescending(asistencia => asistencia.Fecha)
            .ToListAsync();
    }

    public static async Task<int> AgregarAsistenciaAsync(
        Asistencia asistencia)
    {
        await InicializarAsync();

        return await ObtenerConexion().InsertAsync(asistencia);
    }

    public static async Task<bool> ActualizarAsistenciaAsync(
        Asistencia asistenciaActualizada)
    {
        await InicializarAsync();

        var filasActualizadas = await ObtenerConexion()
            .UpdateAsync(asistenciaActualizada);

        return filasActualizadas > 0;
    }

    public static async Task<bool> EliminarAsistenciaAsync(int id)
    {
        await InicializarAsync();

        var filasEliminadas = await ObtenerConexion()
            .DeleteAsync<Asistencia>(id);

        return filasEliminadas > 0;
    }

    // =========================================================
    // CALIFICACIONES
    // =========================================================

    public static async Task<List<Calificacion>>
        ObtenerCalificacionesAsync()
    {
        await InicializarAsync();

        return await ObtenerConexion()
            .Table<Calificacion>()
            .OrderBy(calificacion => calificacion.Estudiante)
            .ThenBy(calificacion => calificacion.Materia)
            .ToListAsync();
    }

    public static async Task<int> AgregarCalificacionAsync(
        Calificacion calificacion)
    {
        await InicializarAsync();

        return await ObtenerConexion().InsertAsync(calificacion);
    }

    public static async Task<bool> ActualizarCalificacionAsync(
        Calificacion calificacionActualizada)
    {
        await InicializarAsync();

        var filasActualizadas = await ObtenerConexion()
            .UpdateAsync(calificacionActualizada);

        return filasActualizadas > 0;
    }

    public static async Task<bool> EliminarCalificacionAsync(int id)
    {
        await InicializarAsync();

        var filasEliminadas = await ObtenerConexion()
            .DeleteAsync<Calificacion>(id);

        return filasEliminadas > 0;
    }

    // =========================================================
    // DATOS INICIALES PARA PRUEBAS
    // =========================================================

    public static async Task CargarDatosDePruebaAsync()
    {
        await InicializarAsync();

        var conexion = ObtenerConexion();

        var cantidadEstudiantes = await conexion
            .Table<Estudiante>()
            .CountAsync();

        var cantidadMaterias = await conexion
            .Table<Materia>()
            .CountAsync();

        if (cantidadEstudiantes > 0 || cantidadMaterias > 0)
        {
            return;
        }

        await conexion.InsertAsync(new Estudiante
        {
            Matricula = "MT-2023-00518",
            Nombre = "Junior",
            Apellido = "Valera",
            Carrera = "Ingeniería de Software",
            Telefono = "809-000-0000"
        });

        await conexion.InsertAsync(new Estudiante
        {
            Matricula = "SD-18-11014",
            Nombre = "Yisel",
            Apellido = "Santana",
            Carrera = "Ingeniería de Software",
            Telefono = "809-000-0001"
        });

        await conexion.InsertAsync(new Materia
        {
            Codigo = "INF-4316",
            Nombre = "Programación de Aplicaciones Móviles",
            Profesor = "Profesor de la asignatura",
            Creditos = 4
        });
    }
}