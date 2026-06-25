namespace RegistroEstudiantes.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private const string UsuarioCorrecto = "admin";
    private const string ContrasenaCorrecta = "1234";

    public LoginPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        LimpiarMensajeError();
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text?.Trim() ?? string.Empty;
        string contrasena = ContrasenaEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(usuario))
        {
            MostrarMensajeError("El usuario es obligatorio.");
            return;
        }

        if (string.IsNullOrWhiteSpace(contrasena))
        {
            MostrarMensajeError("La contraseña es obligatoria.");
            return;
        }

        if (usuario != UsuarioCorrecto || contrasena != ContrasenaCorrecta)
        {
            MostrarMensajeError("Usuario o contraseña incorrectos.");
            return;
        }

        LimpiarMensajeError();

        UsuarioEntry.Text = string.Empty;
        ContrasenaEntry.Text = string.Empty;

        Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

        await Shell.Current.GoToAsync("//home");
    }

    private void MostrarMensajeError(string mensaje)
    {
        MensajeErrorLabel.Text = mensaje;
        MensajeErrorLabel.IsVisible = true;
    }

    private void LimpiarMensajeError()
    {
        MensajeErrorLabel.Text = string.Empty;
        MensajeErrorLabel.IsVisible = false;
    }
}