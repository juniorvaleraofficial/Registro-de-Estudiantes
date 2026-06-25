using RegistroEstudiantes.Mobile.Views;

namespace RegistroEstudiantes.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(StudentsPage), typeof(StudentsPage));
        Routing.RegisterRoute(nameof(StudentFormPage), typeof(StudentFormPage));
        Routing.RegisterRoute(nameof(MateriasPage), typeof(MateriasPage));
        Routing.RegisterRoute(nameof(AsistenciasPage), typeof(AsistenciasPage));
        Routing.RegisterRoute(nameof(CalificacionesPage), typeof(CalificacionesPage));
    }

    private async void OnCerrarSesionClicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert(
            "Cerrar sesión",
            "¿Deseas salir del sistema?",
            "Sí",
            "No");

        if (!confirmar)
        {
            return;
        }

        FlyoutBehavior = FlyoutBehavior.Disabled;
        await GoToAsync("//login");
    }
}