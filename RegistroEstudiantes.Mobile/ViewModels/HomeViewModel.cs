using CommunityToolkit.Mvvm.ComponentModel;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [ObservableProperty]
    private string totalEstudiantes = "0";

    [ObservableProperty]
    private string totalMaterias = "0";

    [ObservableProperty]
    private string promedioGeneral = "Sin notas";

    [ObservableProperty]
    private string porcentajeAsistencia = "Sin registros";

    [ObservableProperty]
    private string descripcionPromedio =
        "Registra calificaciones para calcular el promedio.";

    [ObservableProperty]
    private string descripcionAsistencia =
        "Registra asistencias para calcular el porcentaje.";

    [ObservableProperty]
    private bool estaCargando;

    public async Task CargarEstadisticasAsync()
    {
        if (EstaCargando)
        {
            return;
        }

        try
        {
            EstaCargando = true;

            await ServicioAcademico.CargarDatosDePruebaAsync();

            var estudiantesTask =
                ServicioAcademico.ObtenerEstudiantesAsync();

            var materiasTask =
                ServicioAcademico.ObtenerMateriasAsync();

            var calificacionesTask =
                ServicioAcademico.ObtenerCalificacionesAsync();

            var asistenciasTask =
                ServicioAcademico.ObtenerAsistenciasAsync();

            await Task.WhenAll(
                estudiantesTask,
                materiasTask,
                calificacionesTask,
                asistenciasTask);

            var estudiantes = await estudiantesTask;
            var materias = await materiasTask;
            var calificaciones = await calificacionesTask;
            var asistencias = await asistenciasTask;

            TotalEstudiantes = estudiantes.Count.ToString();
            TotalMaterias = materias.Count.ToString();

            if (calificaciones.Count > 0)
            {
                var promedio =
                    calificaciones.Average(
                        calificacion => calificacion.Nota);

                PromedioGeneral = $"{promedio:N1} puntos";
                DescripcionPromedio =
                    $"Calculado con {calificaciones.Count} " +
                    (calificaciones.Count == 1
                        ? "calificación."
                        : "calificaciones.");
            }
            else
            {
                PromedioGeneral = "Sin notas";
                DescripcionPromedio =
                    "Registra calificaciones para calcular el promedio.";
            }

            if (asistencias.Count > 0)
            {
                var cantidadPresentes =
                    asistencias.Count(asistencia =>
                        string.Equals(
                            asistencia.Estado,
                            "Presente",
                            StringComparison.OrdinalIgnoreCase));

                var porcentaje =
                    cantidadPresentes * 100.0 /
                    asistencias.Count;

                PorcentajeAsistencia = $"{porcentaje:N0}%";
                DescripcionAsistencia =
                    $"{cantidadPresentes} de {asistencias.Count} " +
                    "registros están presentes.";
            }
            else
            {
                PorcentajeAsistencia = "Sin registros";
                DescripcionAsistencia =
                    "Registra asistencias para calcular el porcentaje.";
            }
        }
        finally
        {
            EstaCargando = false;
        }
    }
}