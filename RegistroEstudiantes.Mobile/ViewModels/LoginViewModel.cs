using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private const string UsuarioCorrecto = "admin";
    private const string ContrasenaCorrecta = "1234";

    public event Func<Task>? LoginExitoso;

    private bool validacionesActivadas;

    [ObservableProperty]
    private string usuario = string.Empty;

    [ObservableProperty]
    private string contrasena = string.Empty;

    [ObservableProperty]
    private string usuarioError = string.Empty;

    [ObservableProperty]
    private string contrasenaError = string.Empty;

    [ObservableProperty]
    private bool tieneErrorUsuario;

    [ObservableProperty]
    private bool tieneErrorContrasena;

    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        validacionesActivadas = true;

        if (!ValidarFormulario())
        {
            return;
        }

        if (!Usuario.Equals(UsuarioCorrecto, StringComparison.OrdinalIgnoreCase) ||
            Contrasena != ContrasenaCorrecta)
        {
            ContrasenaError = "Usuario o contraseña incorrectos.";
            TieneErrorContrasena = true;
            return;
        }

        LimpiarFormulario();

        if (LoginExitoso is not null)
        {
            await LoginExitoso();
        }
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        validacionesActivadas = false;

        Usuario = string.Empty;
        Contrasena = string.Empty;

        LimpiarErrores();
    }

    private bool ValidarFormulario()
    {
        LimpiarErrores();

        var esValido = true;

        if (string.IsNullOrWhiteSpace(Usuario))
        {
            UsuarioError = "El usuario es obligatorio.";
            TieneErrorUsuario = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Contrasena))
        {
            ContrasenaError = "La contraseña es obligatoria.";
            TieneErrorContrasena = true;
            esValido = false;
        }

        return esValido;
    }

    private void LimpiarErrores()
    {
        UsuarioError = string.Empty;
        ContrasenaError = string.Empty;

        TieneErrorUsuario = false;
        TieneErrorContrasena = false;
    }

    partial void OnUsuarioChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorUsuario)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            UsuarioError = "El usuario es obligatorio.";
            TieneErrorUsuario = true;
            return;
        }

        UsuarioError = string.Empty;
        TieneErrorUsuario = false;
    }

    partial void OnContrasenaChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorContrasena)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            ContrasenaError = "La contraseña es obligatoria.";
            TieneErrorContrasena = true;
            return;
        }

        ContrasenaError = string.Empty;
        TieneErrorContrasena = false;
    }
}