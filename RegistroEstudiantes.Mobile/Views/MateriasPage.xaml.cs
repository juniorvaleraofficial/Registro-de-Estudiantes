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

        viewModel.SolicitarAlerta += async (titulo, mensaje, boton) =>
            await DisplayAlertAsync(titulo, mensaje, boton);
    }
}