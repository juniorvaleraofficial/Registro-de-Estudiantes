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
    }
}