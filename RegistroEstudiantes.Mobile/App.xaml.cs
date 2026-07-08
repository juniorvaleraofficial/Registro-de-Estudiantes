using Microsoft.Maui.Controls;

namespace RegistroEstudiantes.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}