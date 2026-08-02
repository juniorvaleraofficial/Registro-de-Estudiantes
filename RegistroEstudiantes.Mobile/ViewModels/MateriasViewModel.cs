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

    public string TextoCantidadMaterias =>
        Materias.Count == 1
            ? "1 registro"
            : $"{Materias.Count} registros";

    public event Func<string, string, string, Task>? SolicitarAlerta;

    public event Func<string, string, string, string, Task<bool>>?
        SolicitarConfirmacion;

    private bool validacionesActivadas;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TituloFormulario))]
    [NotifyPropertyChangedFor(nameof(TextoBotonGuardar))]
    [NotifyPropertyChangedFor(nameof(TextoBotonSecundario))]
    [NotifyPropertyChangedFor(nameof(MostrarBotonEliminar))]
    private bool esModoEdicion;

    [ObservableProperty]
    private int? idEnEdicion;

    public string TituloFormulario =>
        EsModoEdicion ? "Editar materia" : "Nueva materia";

    public string TextoBotonGuardar =>
        EsModoEdicion ? "Guardar cambios" : "Guardar materia";

    public string TextoBotonSecundario =>
        EsModoEdicion ? "Cancelar edición" : "Limpiar formulario";

    public bool MostrarBotonEliminar => EsModoEdicion;

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

        var estabaEditando = EsModoEdicion;

        if (estabaEditando && IdEnEdicion is null)
        {
            if (SolicitarAlerta is not null)
            {
                await SolicitarAlerta(
                    "No se pudo editar",
                    "No se encontró el identificador de la materia seleccionada.",
                    "Aceptar");
            }

            return;
        }

        var materia = new Materia
        {
            Id = IdEnEdicion ?? 0,
            Codigo = Codigo.Trim().ToUpper(),
            Nombre = Nombre.Trim(),
            Profesor = Profesor.Trim(),
            Creditos = int.Parse(CreditosTexto.Trim())
        };

        if (estabaEditando)
        {
            var actualizada =
                ServicioAcademico.ActualizarMateria(materia);

            if (!actualizada)
            {
                if (SolicitarAlerta is not null)
                {
                    await SolicitarAlerta(
                        "No se pudo actualizar",
                        "La materia seleccionada ya no se encuentra disponible.",
                        "Aceptar");
                }

                return;
            }
        }
        else
        {
            ServicioAcademico.AgregarMateria(materia);
        }

        CargarMaterias();
        LimpiarFormulario();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                estabaEditando
                    ? "Materia actualizada"
                    : "Materia guardada",
                estabaEditando
                    ? "Los cambios de la materia se guardaron correctamente."
                    : "La materia fue registrada correctamente en memoria.",
                "Aceptar");
        }
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        validacionesActivadas = false;

        IdEnEdicion = null;
        EsModoEdicion = false;

        Codigo = string.Empty;
        Nombre = string.Empty;
        Profesor = string.Empty;
        CreditosTexto = string.Empty;

        LimpiarErrores();
    }

    [RelayCommand]
    private void SeleccionarMateria(Materia? materia)
    {
        if (materia is null)
        {
            return;
        }

        validacionesActivadas = false;

        IdEnEdicion = materia.Id;
        Codigo = materia.Codigo;
        Nombre = materia.Nombre;
        Profesor = materia.Profesor;
        CreditosTexto = materia.Creditos.ToString();

        LimpiarErrores();
        EsModoEdicion = true;
    }

    [RelayCommand]
    private async Task EliminarMateriaActualAsync()
    {
        if (IdEnEdicion is null)
        {
            return;
        }

        var materia = ServicioAcademico
            .ObtenerMaterias()
            .FirstOrDefault(item => item.Id == IdEnEdicion.Value);

        if (materia is null)
        {
            if (SolicitarAlerta is not null)
            {
                await SolicitarAlerta(
                    "Materia no encontrada",
                    "El registro seleccionado ya no está disponible.",
                    "Aceptar");
            }

            LimpiarFormulario();
            return;
        }

        await ConfirmarYEliminarMateriaAsync(materia);
    }

    [RelayCommand]
    private async Task EliminarMateriaDesdeListaAsync(
        Materia? materia)
    {
        if (materia is null)
        {
            return;
        }

        await ConfirmarYEliminarMateriaAsync(materia);
    }

    private async Task ConfirmarYEliminarMateriaAsync(
        Materia materia)
    {
        if (SolicitarConfirmacion is null)
        {
            return;
        }

        var confirmado = await SolicitarConfirmacion(
            "Eliminar materia",
            $"¿Seguro que deseas eliminar {materia.DescripcionCorta}?",
            "Eliminar",
            "Cancelar");

        if (!confirmado)
        {
            return;
        }

        var eliminada =
            ServicioAcademico.EliminarMateria(materia.Id);

        if (!eliminada)
        {
            if (SolicitarAlerta is not null)
            {
                await SolicitarAlerta(
                    "No se pudo eliminar",
                    "La materia ya no se encuentra disponible.",
                    "Aceptar");
            }

            return;
        }

        if (IdEnEdicion == materia.Id)
        {
            LimpiarFormulario();
        }

        CargarMaterias();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                "Materia eliminada",
                "El registro fue eliminado correctamente.",
                "Aceptar");
        }
    }

    private void CargarMaterias()
    {
        Materias.Clear();

        foreach (var materia in ServicioAcademico.ObtenerMaterias())
        {
            Materias.Add(materia);
        }

        OnPropertyChanged(nameof(TextoCantidadMaterias));
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

    private bool ExisteCodigoMateria(string codigo)
    {
        return ServicioAcademico
            .ObtenerMaterias()
            .Any(materia =>
                materia.Id != IdEnEdicion &&
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