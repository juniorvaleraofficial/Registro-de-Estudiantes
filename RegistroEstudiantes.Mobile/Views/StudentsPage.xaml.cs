using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.Views;

public partial class StudentsPage : ContentPage
{
    public StudentsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ServicioAcademico.CargarDatosDePrueba();
        CargarEstudiantes();
        LimpiarMensaje();
    }

    private async void OnGuardarEstudianteClicked(object? sender, EventArgs e)
    {
        string matricula = MatriculaEntry.Text?.Trim() ?? string.Empty;
        string nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        string apellido = ApellidoEntry.Text?.Trim() ?? string.Empty;
        string carrera = CarreraEntry.Text?.Trim() ?? string.Empty;
        string telefono = TelefonoEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(matricula))
        {
            MostrarMensaje("La matrícula es obligatoria.");
            return;
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MostrarMensaje("El nombre es obligatorio.");
            return;
        }

        if (string.IsNullOrWhiteSpace(apellido))
        {
            MostrarMensaje("El apellido es obligatorio.");
            return;
        }

        if (string.IsNullOrWhiteSpace(carrera))
        {
            MostrarMensaje("La carrera es obligatoria.");
            return;
        }

        var estudiante = new Estudiante
        {
            Matricula = matricula,
            Nombre = nombre,
            Apellido = apellido,
            Carrera = carrera,
            Telefono = telefono
        };

        ServicioAcademico.AgregarEstudiante(estudiante);

        LimpiarFormulario();
        CargarEstudiantes();
        LimpiarMensaje();

        await DisplayAlertAsync(
            "Estudiante guardado",
            "El estudiante fue registrado correctamente en memoria.",
            "Aceptar");
    }

    private void OnLimpiarFormularioClicked(object? sender, EventArgs e)
    {
        LimpiarFormulario();
        LimpiarMensaje();
    }

    private void CargarEstudiantes()
    {
        EstudiantesCollectionView.ItemsSource = null;
        EstudiantesCollectionView.ItemsSource = ServicioAcademico.ObtenerEstudiantes().ToList();
    }

    private void LimpiarFormulario()
    {
        MatriculaEntry.Text = string.Empty;
        NombreEntry.Text = string.Empty;
        ApellidoEntry.Text = string.Empty;
        CarreraEntry.Text = string.Empty;
        TelefonoEntry.Text = string.Empty;
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