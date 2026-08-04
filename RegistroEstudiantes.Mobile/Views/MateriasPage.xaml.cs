using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class MateriasPage : ContentPage
{
    private readonly MateriasViewModel viewModel;

    public MateriasPage()
    {
        InitializeComponent();

        viewModel = new MateriasViewModel();
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

        await viewModel.CargarMateriasAsync();
    }
}