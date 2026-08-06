using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class PerfilAcademicoViewModel : ObservableObject
{
    public ObservableCollection<Calificacion> Calificaciones { get; } =
        new();

    public ObservableCollection<Asistencia> Asistencias { get; } =
        new();

    [ObservableProperty]
    private Estudiante? estudianteActual;

    [ObservableProperty]
    private string promedioGeneral = "Sin notas";

    [ObservableProperty]
    private string porcentajeAsistencia = "Sin registros";

    [ObservableProperty]
    private string resumenCalificaciones =
        "No hay calificaciones registradas.";

    [ObservableProperty]
    private string resumenAsistencias =
        "No hay asistencias registradas.";

    [ObservableProperty]
    private bool tieneCalificaciones;

    [ObservableProperty]
    private bool tieneAsistencias;

    [ObservableProperty]
    private bool estaCargando;

    public async Task<bool> CargarPerfilAsync(int estudianteId)
    {
        if (EstaCargando)
        {
            return EstudianteActual is not null;
        }

        try
        {
            EstaCargando = true;

            await ServicioAcademico.CargarDatosDePruebaAsync();

            var estudiantesTask =
                ServicioAcademico.ObtenerEstudiantesAsync();

            var calificacionesTask =
                ServicioAcademico.ObtenerCalificacionesAsync();

            var asistenciasTask =
                ServicioAcademico.ObtenerAsistenciasAsync();

            await Task.WhenAll(
                estudiantesTask,
                calificacionesTask,
                asistenciasTask);

            var estudiantes = await estudiantesTask;

            EstudianteActual = estudiantes.FirstOrDefault(
                estudiante => estudiante.Id == estudianteId);

            if (EstudianteActual is null)
            {
                LimpiarResultados();
                return false;
            }

            var nombreCompleto = EstudianteActual.NombreCompleto.Trim();

            var calificacionesEstudiante =
                (await calificacionesTask)
                .Where(calificacion =>
                    string.Equals(
                        calificacion.Estudiante.Trim(),
                        nombreCompleto,
                        StringComparison.OrdinalIgnoreCase))
                .OrderBy(calificacion => calificacion.Materia)
                .ToList();

            var asistenciasEstudiante =
                (await asistenciasTask)
                .Where(asistencia =>
                    string.Equals(
                        asistencia.Estudiante.Trim(),
                        nombreCompleto,
                        StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(asistencia => asistencia.Fecha)
                .ToList();

            CargarCalificaciones(calificacionesEstudiante);
            CargarAsistencias(asistenciasEstudiante);

            return true;
        }
        finally
        {
            EstaCargando = false;
        }
    }

    private void CargarCalificaciones(
        IReadOnlyCollection<Calificacion> calificaciones)
    {
        Calificaciones.Clear();

        foreach (var calificacion in calificaciones)
        {
            Calificaciones.Add(calificacion);
        }

        TieneCalificaciones = Calificaciones.Count > 0;

        if (!TieneCalificaciones)
        {
            PromedioGeneral = "Sin notas";
            ResumenCalificaciones =
                "No hay calificaciones registradas.";
            return;
        }

        var promedio = Calificaciones.Average(
            calificacion => calificacion.Nota);

        PromedioGeneral = $"{promedio:N1} puntos";

        ResumenCalificaciones =
            Calificaciones.Count == 1
                ? "1 calificación registrada."
                : $"{Calificaciones.Count} calificaciones registradas.";
    }

    private void CargarAsistencias(
        IReadOnlyCollection<Asistencia> asistencias)
    {
        Asistencias.Clear();

        foreach (var asistencia in asistencias)
        {
            Asistencias.Add(asistencia);
        }

        TieneAsistencias = Asistencias.Count > 0;

        if (!TieneAsistencias)
        {
            PorcentajeAsistencia = "Sin registros";
            ResumenAsistencias =
                "No hay asistencias registradas.";
            return;
        }

        var cantidadPresentes = Asistencias.Count(asistencia =>
            string.Equals(
                asistencia.Estado,
                "Presente",
                StringComparison.OrdinalIgnoreCase));

        var porcentaje =
            cantidadPresentes * 100.0 / Asistencias.Count;

        PorcentajeAsistencia = $"{porcentaje:N0}%";

        ResumenAsistencias =
            $"{cantidadPresentes} de {Asistencias.Count} " +
            "registros están presentes.";
    }

    private void LimpiarResultados()
    {
        EstudianteActual = null;

        Calificaciones.Clear();
        Asistencias.Clear();

        PromedioGeneral = "Sin notas";
        PorcentajeAsistencia = "Sin registros";

        TieneCalificaciones = false;
        TieneAsistencias = false;

        ResumenCalificaciones =
            "No hay calificaciones registradas.";

        ResumenAsistencias =
            "No hay asistencias registradas.";
    }
}