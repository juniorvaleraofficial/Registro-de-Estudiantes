using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class StudentsPage : ContentPage
{
    private readonly StudentsViewModel viewModel;

    public StudentsPage()
    {
        InitializeComponent();

        viewModel = new StudentsViewModel();
        BindingContext = viewModel;

        viewModel.SolicitarAlerta += async (
            titulo,
            mensaje,
            boton) =>
        {
            await DisplayAlertAsync(
                titulo,
                mensaje,
                boton);
        };

        viewModel.SolicitarConfirmacion += async (
            titulo,
            mensaje,
            botonAceptar,
            botonCancelar) =>
        {
            return await DisplayAlertAsync(
                titulo,
                mensaje,
                botonAceptar,
                botonCancelar);
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await viewModel.CargarDatosAsync();
    }

    private async void OnVerPerfilClicked(
        object? sender,
        EventArgs e)
    {
        if (sender is not Button boton ||
            boton.CommandParameter is not Estudiante estudiante)
        {
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(PerfilAcademicoPage)}?id={estudiante.Id}");
    }
}