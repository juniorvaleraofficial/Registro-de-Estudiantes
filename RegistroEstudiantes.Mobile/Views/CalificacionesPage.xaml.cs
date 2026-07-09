using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class CalificacionesPage : ContentPage
{
    private readonly CalificacionesViewModel viewModel;

    public CalificacionesPage()
    {
        InitializeComponent();

        viewModel = new CalificacionesViewModel();
        BindingContext = viewModel;

        viewModel.SolicitarAlerta += async (titulo, mensaje, boton) =>
            await DisplayAlertAsync(titulo, mensaje, boton);
    }
}