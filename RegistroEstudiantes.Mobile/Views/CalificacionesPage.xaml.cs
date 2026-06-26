using System.Globalization;
using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.Views;

public partial class CalificacionesPage : ContentPage
{
    public CalificacionesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ServicioAcademico.CargarDatosDePrueba();
        CargarOpciones();
        CargarCalificaciones();
        LimpiarMensaje();
    }

    private async void OnGuardarCalificacionClicked(object? sender, EventArgs e)
    {
        string estudiante = EstudiantePicker.SelectedItem?.ToString() ?? string.Empty;
        string materia = MateriaPicker.SelectedItem?.ToString() ?? string.Empty;
        string notaTexto = NotaEntry.Text?.Trim() ?? string.Empty;
        string observacion = ObservacionEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(estudiante))
        {
            MostrarMensaje("Debes seleccionar un estudiante.");
            return;
        }

        if (string.IsNullOrWhiteSpace(materia))
        {
            MostrarMensaje("Debes seleccionar una materia.");
            return;
        }

        if (string.IsNullOrWhiteSpace(notaTexto))
        {
            MostrarMensaje("La nota es obligatoria.");
            return;
        }

        string notaNormalizada = notaTexto.Replace(',', '.');

        if (!double.TryParse(notaNormalizada, NumberStyles.Number, CultureInfo.InvariantCulture, out double nota))
        {
            MostrarMensaje("La nota debe ser un número válido.");
            return;
        }

        if (nota < 0 || nota > 100)
        {
            MostrarMensaje("La nota debe estar entre 0 y 100.");
            return;
        }

        var calificacion = new Calificacion
        {
            Estudiante = estudiante,
            Materia = materia,
            Nota = nota,
            Observacion = observacion
        };

        ServicioAcademico.AgregarCalificacion(calificacion);

        LimpiarFormulario();
        CargarCalificaciones();
        LimpiarMensaje();

        await DisplayAlertAsync(
            "Calificación guardada",
            "La calificación fue registrada correctamente en memoria.",
            "Aceptar");
    }

    private void OnLimpiarFormularioClicked(object? sender, EventArgs e)
    {
        LimpiarFormulario();
        LimpiarMensaje();
    }

    private void CargarOpciones()
    {
        EstudiantePicker.ItemsSource = ServicioAcademico
            .ObtenerEstudiantes()
            .Select(estudiante => estudiante.NombreCompleto)
            .ToList();

        MateriaPicker.ItemsSource = ServicioAcademico
            .ObtenerMaterias()
            .Select(materia => materia.DescripcionCorta)
            .ToList();
    }

    private void CargarCalificaciones()
    {
        CalificacionesCollectionView.ItemsSource = null;
        CalificacionesCollectionView.ItemsSource = ServicioAcademico.ObtenerCalificaciones().ToList();
    }

    private void LimpiarFormulario()
    {
        EstudiantePicker.SelectedIndex = -1;
        MateriaPicker.SelectedIndex = -1;
        NotaEntry.Text = string.Empty;
        ObservacionEntry.Text = string.Empty;
    }

    private void MostrarMensaje(string mensaje)
    {
        MensajeLabel.Text = mensaje;
        MensajeLabel.TextColor = Color.FromArgb("#DC2626");
        MensajeLabel.IsVisible = true;
    }

    private void LimpiarMensaje()
    {
        MensajeLabel.Text = string.Empty;
        MensajeLabel.IsVisible = false;
    }
}