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

        viewModel.SolicitarAlerta += async (titulo, mensaje, boton) =>
            await DisplayAlertAsync(titulo, mensaje, boton);
    }
}