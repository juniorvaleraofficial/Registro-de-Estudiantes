using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class AsistenciasPage : ContentPage
{
    private readonly AsistenciasViewModel viewModel;

    public AsistenciasPage()
    {
        InitializeComponent();

        viewModel = new AsistenciasViewModel();
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
        await ContenidoScrollView.ScrollToAsync(
            0,
            0,
            false);
    }
}