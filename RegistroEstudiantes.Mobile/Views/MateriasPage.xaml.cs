using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.Views;

public partial class MateriasPage : ContentPage
{
    public MateriasPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ServicioAcademico.CargarDatosDePrueba();
        CargarMaterias();
        LimpiarMensaje();
    }

    private async void OnGuardarMateriaClicked(object? sender, EventArgs e)
    {
        string codigo = CodigoEntry.Text?.Trim() ?? string.Empty;
        string nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        string profesor = ProfesorEntry.Text?.Trim() ?? string.Empty;
        string creditosTexto = CreditosEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(codigo))
        {
            MostrarMensaje("El código de la materia es obligatorio.");
            return;
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MostrarMensaje("El nombre de la materia es obligatorio.");
            return;
        }

        if (string.IsNullOrWhiteSpace(profesor))
        {
            MostrarMensaje("El profesor es obligatorio.");
            return;
        }

        if (!int.TryParse(creditosTexto, out int creditos) || creditos <= 0)
        {
            MostrarMensaje("Los créditos deben ser un número mayor que cero.");
            return;
        }

        var materia = new Materia
        {
            Codigo = codigo,
            Nombre = nombre,
            Profesor = profesor,
            Creditos = creditos
        };

        ServicioAcademico.AgregarMateria(materia);

        LimpiarFormulario();
        CargarMaterias();
        LimpiarMensaje();

        await DisplayAlertAsync(
            "Materia guardada",
            "La materia fue registrada correctamente en memoria.",
            "Aceptar");
    }

    private void OnLimpiarFormularioClicked(object? sender, EventArgs e)
    {
        LimpiarFormulario();
        LimpiarMensaje();
    }

    private void CargarMaterias()
    {
        MateriasCollectionView.ItemsSource = null;
        MateriasCollectionView.ItemsSource = ServicioAcademico.ObtenerMaterias().ToList();
    }

    private void LimpiarFormulario()
    {
        CodigoEntry.Text = string.Empty;
        NombreEntry.Text = string.Empty;
        ProfesorEntry.Text = string.Empty;
        CreditosEntry.Text = string.Empty;
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