using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class MateriasViewModel : ObservableObject
{
    public ObservableCollection<Materia> Materias { get; } = new();

    public event Func<string, string, string, Task>? SolicitarAlerta;

    private bool validacionesActivadas;

    [ObservableProperty]
    private string codigo = string.Empty;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string profesor = string.Empty;

    [ObservableProperty]
    private string creditosTexto = string.Empty;

    [ObservableProperty]
    private string codigoError = string.Empty;

    [ObservableProperty]
    private string nombreError = string.Empty;

    [ObservableProperty]
    private string profesorError = string.Empty;

    [ObservableProperty]
    private string creditosError = string.Empty;

    [ObservableProperty]
    private bool tieneErrorCodigo;

    [ObservableProperty]
    private bool tieneErrorNombre;

    [ObservableProperty]
    private bool tieneErrorProfesor;

    [ObservableProperty]
    private bool tieneErrorCreditos;

    public MateriasViewModel()
    {
        ServicioAcademico.CargarDatosDePrueba();
        CargarMaterias();
    }

    [RelayCommand]
    private async Task GuardarMateriaAsync()
    {
        validacionesActivadas = true;

        if (!ValidarFormulario())
        {
            return;
        }

        var materia = new Materia
        {
            Codigo = Codigo.Trim().ToUpper(),
            Nombre = Nombre.Trim(),
            Profesor = Profesor.Trim(),
            Creditos = int.Parse(CreditosTexto.Trim())
        };

        ServicioAcademico.AgregarMateria(materia);

        CargarMaterias();
        LimpiarFormulario();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                "Materia guardada",
                "La materia fue registrada correctamente en memoria.",
                "Aceptar");
        }
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        validacionesActivadas = false;

        Codigo = string.Empty;
        Nombre = string.Empty;
        Profesor = string.Empty;
        CreditosTexto = string.Empty;

        LimpiarErrores();
    }

    private void CargarMaterias()
    {
        Materias.Clear();

        foreach (var materia in ServicioAcademico.ObtenerMaterias())
        {
            Materias.Add(materia);
        }
    }

    private bool ValidarFormulario()
    {
        LimpiarErrores();

        var esValido = true;

        if (string.IsNullOrWhiteSpace(Codigo))
        {
            CodigoError = "El código de la materia es obligatorio.";
            TieneErrorCodigo = true;
            esValido = false;
        }
        else if (!CodigoTieneFormatoValido(Codigo))
        {
            CodigoError = "Use un código válido, por ejemplo INF-4316.";
            TieneErrorCodigo = true;
            esValido = false;
        }
        else if (ExisteCodigoMateria(Codigo))
        {
            CodigoError = "Ya existe una materia con este código.";
            TieneErrorCodigo = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Nombre))
        {
            NombreError = "El nombre de la materia es obligatorio.";
            TieneErrorNombre = true;
            esValido = false;
        }
        else if (Nombre.Trim().Length < 3)
        {
            NombreError = "El nombre debe tener al menos 3 caracteres.";
            TieneErrorNombre = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(Profesor))
        {
            ProfesorError = "El profesor es obligatorio.";
            TieneErrorProfesor = true;
            esValido = false;
        }
        else if (Profesor.Trim().Length < 3)
        {
            ProfesorError = "El nombre del profesor debe tener al menos 3 caracteres.";
            TieneErrorProfesor = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(CreditosTexto))
        {
            CreditosError = "Los créditos son obligatorios.";
            TieneErrorCreditos = true;
            esValido = false;
        }
        else if (!int.TryParse(CreditosTexto.Trim(), out int creditos))
        {
            CreditosError = "Los créditos deben ser un número.";
            TieneErrorCreditos = true;
            esValido = false;
        }
        else if (creditos <= 0)
        {
            CreditosError = "Los créditos deben ser mayores que cero.";
            TieneErrorCreditos = true;
            esValido = false;
        }
        else if (creditos > 6)
        {
            CreditosError = "Los créditos no deben ser mayores de 6.";
            TieneErrorCreditos = true;
            esValido = false;
        }

        return esValido;
    }

    private static bool CodigoTieneFormatoValido(string codigo)
    {
        return Regex.IsMatch(
            codigo.Trim(),
            @"^[A-Z]{2,4}-\d{3,4}$",
            RegexOptions.IgnoreCase);
    }

    private static bool ExisteCodigoMateria(string codigo)
    {
        return ServicioAcademico
            .ObtenerMaterias()
            .Any(materia =>
                materia.Codigo.Equals(
                    codigo.Trim(),
                    StringComparison.OrdinalIgnoreCase));
    }

    private void LimpiarErrores()
    {
        CodigoError = string.Empty;
        NombreError = string.Empty;
        ProfesorError = string.Empty;
        CreditosError = string.Empty;

        TieneErrorCodigo = false;
        TieneErrorNombre = false;
        TieneErrorProfesor = false;
        TieneErrorCreditos = false;
    }

    partial void OnCodigoChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorCodigo)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            CodigoError = "El código de la materia es obligatorio.";
            TieneErrorCodigo = true;
            return;
        }

        if (!CodigoTieneFormatoValido(value))
        {
            CodigoError = "Use un código válido, por ejemplo INF-4316.";
            TieneErrorCodigo = true;
            return;
        }

        if (ExisteCodigoMateria(value))
        {
            CodigoError = "Ya existe una materia con este código.";
            TieneErrorCodigo = true;
            return;
        }

        CodigoError = string.Empty;
        TieneErrorCodigo = false;
    }

    partial void OnNombreChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorNombre)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            NombreError = "El nombre de la materia es obligatorio.";
            TieneErrorNombre = true;
            return;
        }

        if (value.Trim().Length < 3)
        {
            NombreError = "El nombre debe tener al menos 3 caracteres.";
            TieneErrorNombre = true;
            return;
        }

        NombreError = string.Empty;
        TieneErrorNombre = false;
    }

    partial void OnProfesorChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorProfesor)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            ProfesorError = "El profesor es obligatorio.";
            TieneErrorProfesor = true;
            return;
        }

        if (value.Trim().Length < 3)
        {
            ProfesorError = "El nombre del profesor debe tener al menos 3 caracteres.";
            TieneErrorProfesor = true;
            return;
        }

        ProfesorError = string.Empty;
        TieneErrorProfesor = false;
    }

    partial void OnCreditosTextoChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorCreditos)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            CreditosError = "Los créditos son obligatorios.";
            TieneErrorCreditos = true;
            return;
        }

        if (!int.TryParse(value.Trim(), out int creditos))
        {
            CreditosError = "Los créditos deben ser un número.";
            TieneErrorCreditos = true;
            return;
        }

        if (creditos <= 0)
        {
            CreditosError = "Los créditos deben ser mayores que cero.";
            TieneErrorCreditos = true;
            return;
        }

        if (creditos > 6)
        {
            CreditosError = "Los créditos no deben ser mayores de 6.";
            TieneErrorCreditos = true;
            return;
        }

        CreditosError = string.Empty;
        TieneErrorCreditos = false;
    }
}