using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.Views;

public partial class AsistenciasPage : ContentPage
{
    public AsistenciasPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ServicioAcademico.CargarDatosDePrueba();
        CargarOpciones();
        CargarAsistencias();
        LimpiarMensaje();
    }

    private async void OnGuardarAsistenciaClicked(object? sender, EventArgs e)
    {
        string estudiante = EstudiantePicker.SelectedItem?.ToString() ?? string.Empty;
        string materia = MateriaPicker.SelectedItem?.ToString() ?? string.Empty;
        string estado = EstadoPicker.SelectedItem?.ToString() ?? string.Empty;

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

        if (string.IsNullOrWhiteSpace(estado))
        {
            MostrarMensaje("Debes seleccionar el estado de asistencia.");
            return;
        }

        var asistencia = new Asistencia
        {
            Estudiante = estudiante,
            Materia = materia,
            Fecha = FechaDatePicker.Date ?? DateTime.Today,
            Estado = estado
        };

        ServicioAcademico.AgregarAsistencia(asistencia);

        LimpiarFormulario();
        CargarAsistencias();
        LimpiarMensaje();

        await DisplayAlertAsync(
            "Asistencia guardada",
            "La asistencia fue registrada correctamente en memoria.",
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

        EstadoPicker.ItemsSource = new List<string>
        {
            "Presente",
            "Ausente",
            "Excusa"
        };
    }

    private void CargarAsistencias()
    {
        AsistenciasCollectionView.ItemsSource = null;
        AsistenciasCollectionView.ItemsSource = ServicioAcademico.ObtenerAsistencias().ToList();
    }

    private void LimpiarFormulario()
    {
        EstudiantePicker.SelectedIndex = -1;
        MateriaPicker.SelectedIndex = -1;
        EstadoPicker.SelectedIndex = -1;
        FechaDatePicker.Date = DateTime.Today;
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