using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel viewModel;

    public HomePage()
    {
        InitializeComponent();

        viewModel = new HomeViewModel();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await viewModel.CargarEstadisticasAsync();
    }

    private async void OnEstudiantesClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//students");
    }

    private async void OnMateriasClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//materias");
    }

    private async void OnAsistenciasClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//asistencias");
    }

    private async void OnCalificacionesClicked(
        object? sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("//calificaciones");
    }
}