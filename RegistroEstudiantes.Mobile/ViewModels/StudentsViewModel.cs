using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class StudentsViewModel : ObservableObject
{
    public ObservableCollection<Estudiante> Estudiantes { get; } = new();

    public event Func<string, string, string, Task>? SolicitarAlerta;

    private bool validacionesActivadas;

    [ObservableProperty]
    private string matricula = string.Empty;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string apellido = string.Empty;

    [ObservableProperty]
    private string carrera = string.Empty;

    [ObservableProperty]
    private string telefono = string.Empty;

    [ObservableProperty]
    private string matriculaError = string.Empty;

    [ObservableProperty]
    private string nombreError = string.Empty;

    [ObservableProperty]
    private string apellidoError = string.Empty;

    [ObservableProperty]
    private string carreraError = string.Empty;

    [ObservableProperty]
    private string telefonoError = string.Empty;

    [ObservableProperty]
    private bool tieneErrorMatricula;

    [ObservableProperty]
    private bool tieneErrorNombre;

    [ObservableProperty]
    private bool tieneErrorApellido;

    [ObservableProperty]
    private bool tieneErrorCarrera;

    [ObservableProperty]
    private bool tieneErrorTelefono;

    public StudentsViewModel()
    {
        ServicioAcademico.CargarDatosDePrueba();
        CargarEstudiantes();
    }

    [RelayCommand]
    private async Task GuardarEstudianteAsync()
    {
        validacionesActivadas = true;

        if (!ValidarFormulario())
        {
            return;
        }

        var estudiante = new Estudiante
        {
            Matricula = Matricula.Trim(),
            Nombre = Nombre.Trim(),
            Apellido = Apellido.Trim(),
            Carrera = Carrera.Trim(),
            Telefono = Telefono.Trim()
        };

        ServicioAcademico.AgregarEstudiante(estudiante);

        CargarEstudiantes();
        LimpiarFormulario();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                "Estudiante guardado",
                "El estudiante fue registrado correctamente en memoria.",
                "Aceptar");
        }
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        validacionesActivadas = false;

        Matricula = string.Empty;
        Nombre = string.Empty;
        Apellido = string.Empty;
        Carrera = string.Empty;
        Telefono = string.Empty;

        LimpiarErrores();
    }

    private void CargarEstudiantes()
    {
        Estudiantes.Clear();

        foreach (var estudiante in ServicioAcademico.ObtenerEstudiantes())
        {
            Estudiantes.Add(estudiante);
        }
    }

    private bool ValidarFormulario()
    {
        LimpiarErrores();

        var esValido = true;

        if (string.IsNullOrWhiteSpace(Matricula))
        {
            MatriculaError = "La matrícula es obligatoria.";
            TieneErrorMatricula = true;
            esValido = false;
        }
        else if (!MatriculaTieneFormatoValido(Matricula))
        {
            MatriculaError = "Use un formato válido, por ejemplo MT-2023-00518.";
            TieneErrorMatricula = true;
            esValido = false;
        }
        else if (ExisteMatricula(Matricula))
        {
            MatriculaError = "Ya existe un estudiante con esta matrícula.";
            TieneErrorMatricula = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Nombre))
        {
            NombreError = "El nombre es obligatorio.";
            TieneErrorNombre = true;
            esValido = false;
        }
        else if (Nombre.Trim().Length < 2)
        {
            NombreError = "El nombre debe tener al menos 2 letras.";
            TieneErrorNombre = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Apellido))
        {
            ApellidoError = "El apellido es obligatorio.";
            TieneErrorApellido = true;
            esValido = false;
        }
        else if (Apellido.Trim().Length < 2)
        {
            ApellidoError = "El apellido debe tener al menos 2 letras.";
            TieneErrorApellido = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Carrera))
        {
            CarreraError = "La carrera es obligatoria.";
            TieneErrorCarrera = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Telefono))
        {
            TelefonoError = "El teléfono es obligatorio.";
            TieneErrorTelefono = true;
            esValido = false;
        }
        else if (!TelefonoTieneDiezDigitos(Telefono))
        {
            TelefonoError = "El teléfono debe tener 10 dígitos.";
            TieneErrorTelefono = true;
            esValido = false;
        }

        return esValido;
    }

    private static bool TelefonoTieneDiezDigitos(string telefono)
    {
        var soloNumeros = new string(telefono.Where(char.IsDigit).ToArray());

        return soloNumeros.Length == 10;
    }

    private static bool MatriculaTieneFormatoValido(string matricula)
    {
        return Regex.IsMatch(
            matricula.Trim(),
            @"^[A-Z]{2}-\d{2,4}-\d{5}$",
            RegexOptions.IgnoreCase);
    }

    private static bool ExisteMatricula(string matricula)
    {
        return ServicioAcademico
            .ObtenerEstudiantes()
            .Any(estudiante =>
                estudiante.Matricula.Equals(
                    matricula.Trim(),
                    StringComparison.OrdinalIgnoreCase));
    }

    private void LimpiarErrores()
    {
        MatriculaError = string.Empty;
        NombreError = string.Empty;
        ApellidoError = string.Empty;
        CarreraError = string.Empty;
        TelefonoError = string.Empty;

        TieneErrorMatricula = false;
        TieneErrorNombre = false;
        TieneErrorApellido = false;
        TieneErrorCarrera = false;
        TieneErrorTelefono = false;
    }
    partial void OnMatriculaChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorMatricula)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            MatriculaError = "La matrícula es obligatoria.";
            TieneErrorMatricula = true;
            return;
        }

        if (!MatriculaTieneFormatoValido(value))
        {
            MatriculaError = "Use un formato válido, por ejemplo MT-2023-00518.";
            TieneErrorMatricula = true;
            return;
        }

        if (ExisteMatricula(value))
        {
            MatriculaError = "Ya existe un estudiante con esta matrícula.";
            TieneErrorMatricula = true;
            return;
        }

        MatriculaError = string.Empty;
        TieneErrorMatricula = false;
    }

    partial void OnNombreChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorNombre)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            NombreError = "El nombre es obligatorio.";
            TieneErrorNombre = true;
            return;
        }

        if (value.Trim().Length < 2)
        {
            NombreError = "El nombre debe tener al menos 2 letras.";
            TieneErrorNombre = true;
            return;
        }

        NombreError = string.Empty;
        TieneErrorNombre = false;
    }

    partial void OnApellidoChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorApellido)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            ApellidoError = "El apellido es obligatorio.";
            TieneErrorApellido = true;
            return;
        }

        if (value.Trim().Length < 2)
        {
            ApellidoError = "El apellido debe tener al menos 2 letras.";
            TieneErrorApellido = true;
            return;
        }

        ApellidoError = string.Empty;
        TieneErrorApellido = false;
    }

    partial void OnCarreraChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorCarrera)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            CarreraError = "La carrera es obligatoria.";
            TieneErrorCarrera = true;
            return;
        }

        CarreraError = string.Empty;
        TieneErrorCarrera = false;
    }

    partial void OnTelefonoChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorTelefono)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            TelefonoError = "El teléfono es obligatorio.";
            TieneErrorTelefono = true;
            return;
        }

        if (!TelefonoTieneDiezDigitos(value))
        {
            TelefonoError = "El teléfono debe tener 10 dígitos.";
            TieneErrorTelefono = true;
            return;
        }

        TelefonoError = string.Empty;
        TieneErrorTelefono = false;
    }

}