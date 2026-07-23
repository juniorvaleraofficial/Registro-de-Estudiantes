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
}