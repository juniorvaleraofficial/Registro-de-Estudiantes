namespace RegistroEstudiantes.Mobile.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnEstudiantesClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//students");
    }

    private async void OnMateriasClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//materias");
    }

    private async void OnAsistenciasClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//asistencias");
    }

    private async void OnCalificacionesClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//calificaciones");
    }
}