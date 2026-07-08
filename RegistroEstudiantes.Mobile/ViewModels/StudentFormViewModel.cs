using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class StudentFormViewModel : ObservableObject
{
    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string apellido = string.Empty;

    [ObservableProperty]
    private string matricula = string.Empty;

    [ObservableProperty]
    private string correo = string.Empty;

    [ObservableProperty]
    private string telefono = string.Empty;

    [ObservableProperty]
    private string carrera = string.Empty;

    [ObservableProperty]
    private DateTime fechaNacimiento = DateTime.Today.AddYears(-18);

    public ObservableCollection<string> Estados { get; } =
    [
        "Activo",
        "Inactivo"
    ];

    [ObservableProperty]
    private string estadoSeleccionado = "Activo";

    [ObservableProperty]
    private string nombreError = string.Empty;

    [ObservableProperty]
    private string apellidoError = string.Empty;

    [ObservableProperty]
    private string matriculaError = string.Empty;

    [ObservableProperty]
    private string correoError = string.Empty;

    [ObservableProperty]
    private string telefonoError = string.Empty;

    [ObservableProperty]
    private string carreraError = string.Empty;

    [ObservableProperty]
    private string fechaNacimientoError = string.Empty;

    [ObservableProperty]
    private string estadoError = string.Empty;

    [ObservableProperty]
    private bool tieneErrorNombre;

    [ObservableProperty]
    private bool tieneErrorApellido;

    [ObservableProperty]
    private bool tieneErrorMatricula;

    [ObservableProperty]
    private bool tieneErrorCorreo;

    [ObservableProperty]
    private bool tieneErrorTelefono;

    [ObservableProperty]
    private bool tieneErrorCarrera;

    [ObservableProperty]
    private bool tieneErrorFechaNacimiento;

    [ObservableProperty]
    private bool tieneErrorEstado;

    [RelayCommand]
    private async Task GuardarEstudianteAsync()
    {
        if (!ValidarFormulario())
        {
            return;
        }

        await Shell.Current.DisplayAlert(
            "Estudiante guardado",
            "La información del estudiante fue validada correctamente.",
            "Aceptar");

        LimpiarFormulario();
    }

    private bool ValidarFormulario()
    {
        LimpiarErrores();

        var esValido = true;

        if (string.IsNullOrWhiteSpace(Nombre))
        {
            NombreError = "El nombre es obligatorio.";
            TieneErrorNombre = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Apellido))
        {
            ApellidoError = "El apellido es obligatorio.";
            TieneErrorApellido = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Matricula))
        {
            MatriculaError = "La matrícula es obligatoria.";
            TieneErrorMatricula = true;
            esValido = false;
        }
        else if (Matricula.Trim().Length < 5)
        {
            MatriculaError = "La matrícula debe tener al menos 5 caracteres.";
            TieneErrorMatricula = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Correo))
        {
            CorreoError = "El correo electrónico es obligatorio.";
            TieneErrorCorreo = true;
            esValido = false;
        }
        else if (!EsCorreoValido(Correo))
        {
            CorreoError = "Ingrese un correo electrónico válido.";
            TieneErrorCorreo = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Telefono))
        {
            TelefonoError = "El teléfono es obligatorio.";
            TieneErrorTelefono = true;
            esValido = false;
        }
        else if (!EsTelefonoValido(Telefono))
        {
            TelefonoError = "El teléfono debe tener 10 dígitos.";
            TieneErrorTelefono = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Carrera))
        {
            CarreraError = "La carrera es obligatoria.";
            TieneErrorCarrera = true;
            esValido = false;
        }

        if (FechaNacimiento > DateTime.Today.AddYears(-12))
        {
            FechaNacimientoError = "El estudiante debe tener al menos 12 años.";
            TieneErrorFechaNacimiento = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(EstadoSeleccionado))
        {
            EstadoError = "Debe seleccionar un estado.";
            TieneErrorEstado = true;
            esValido = false;
        }

        return esValido;
    }

    private static bool EsCorreoValido(string correo)
    {
        return Regex.IsMatch(
            correo.Trim(),
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase);
    }

    private static bool EsTelefonoValido(string telefono)
    {
        var soloNumeros = new string(telefono.Where(char.IsDigit).ToArray());

        return soloNumeros.Length == 10;
    }

    private void LimpiarFormulario()
    {
        Nombre = string.Empty;
        Apellido = string.Empty;
        Matricula = string.Empty;
        Correo = string.Empty;
        Telefono = string.Empty;
        Carrera = string.Empty;
        FechaNacimiento = DateTime.Today.AddYears(-18);
        EstadoSeleccionado = "Activo";

        LimpiarErrores();
    }

    private void LimpiarErrores()
    {
        NombreError = string.Empty;
        ApellidoError = string.Empty;
        MatriculaError = string.Empty;
        CorreoError = string.Empty;
        TelefonoError = string.Empty;
        CarreraError = string.Empty;
        FechaNacimientoError = string.Empty;
        EstadoError = string.Empty;

        TieneErrorNombre = false;
        TieneErrorApellido = false;
        TieneErrorMatricula = false;
        TieneErrorCorreo = false;
        TieneErrorTelefono = false;
        TieneErrorCarrera = false;
        TieneErrorFechaNacimiento = false;
        TieneErrorEstado = false;
    }
}