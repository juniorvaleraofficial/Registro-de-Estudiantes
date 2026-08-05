using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;
using SQLite;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class CalificacionesViewModel : ObservableObject
{
    public ObservableCollection<string> EstudiantesOpciones { get; } = new();
    public ObservableCollection<string> MateriasOpciones { get; } = new();
    public ObservableCollection<Calificacion> Calificaciones { get; } = new();

    public string TextoCantidadCalificaciones =>
        Calificaciones.Count == 1
            ? "1 registro"
            : $"{Calificaciones.Count} registros";

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
        EsModoEdicion
            ? "Editar calificación"
            : "Nueva calificación";

    public string TextoBotonGuardar =>
        EsModoEdicion
            ? "Guardar cambios"
            : "Guardar calificación";

    public string TextoBotonSecundario =>
        EsModoEdicion
            ? "Cancelar edición"
            : "Limpiar formulario";

    public bool MostrarBotonEliminar => EsModoEdicion;

    [ObservableProperty]
    private string? estudianteSeleccionado;

    [ObservableProperty]
    private string? materiaSeleccionada;

    [ObservableProperty]
    private string notaTexto = string.Empty;

    [ObservableProperty]
    private string observacion = string.Empty;

    [ObservableProperty]
    private string estudianteError = string.Empty;

    [ObservableProperty]
    private string materiaError = string.Empty;

    [ObservableProperty]
    private string notaError = string.Empty;

    [ObservableProperty]
    private string observacionError = string.Empty;

    [ObservableProperty]
    private bool tieneErrorEstudiante;

    [ObservableProperty]
    private bool tieneErrorMateria;

    [ObservableProperty]
    private bool tieneErrorNota;

    [ObservableProperty]
    private bool tieneErrorObservacion;

    public CalificacionesViewModel()
    {
    }

    public async Task CargarDatosAsync()
    {
        await ServicioAcademico.CargarDatosDePruebaAsync();
        await CargarOpcionesAsync();
        await CargarCalificacionesAsync();
    }

    [RelayCommand]
    private async Task GuardarCalificacionAsync()
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
                    "No se encontró el identificador de la calificación seleccionada.",
                    "Aceptar");
            }

            return;
        }

        var nota = ConvertirNota(NotaTexto);

        var calificacion = new Calificacion
        {
            Id = IdEnEdicion ?? 0,
            Estudiante =
                EstudianteSeleccionado?.Trim() ?? string.Empty,
            Materia =
                MateriaSeleccionada?.Trim() ?? string.Empty,
            Nota = nota,
            Observacion = Observacion.Trim()
        };

        try
        {
            if (estabaEditando)
            {
                var actualizada =
                    await ServicioAcademico.ActualizarCalificacionAsync(
                        calificacion);

                if (!actualizada)
                {
                    if (SolicitarAlerta is not null)
                    {
                        await SolicitarAlerta(
                            "No se pudo actualizar",
                            "La calificación seleccionada ya no se encuentra disponible.",
                            "Aceptar");
                    }

                    return;
                }
            }
            else
            {
                await ServicioAcademico.AgregarCalificacionAsync(
                    calificacion);
            }
        }
        catch (SQLiteException excepcion)
            when (excepcion.Result == SQLite3.Result.Constraint)
        {
            NotaError =
                "Ya existe una calificación para este estudiante y esta materia.";

            TieneErrorNota = true;
            return;
        }

        await CargarCalificacionesAsync();
        LimpiarFormulario();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                estabaEditando
                    ? "Calificación actualizada"
                    : "Calificación guardada",
                estabaEditando
                    ? "Los cambios de la calificación se guardaron correctamente."
                    : "La calificación fue guardada correctamente en la base de datos.",
                "Aceptar");
        }
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        validacionesActivadas = false;

        IdEnEdicion = null;
        EsModoEdicion = false;

        EstudianteSeleccionado = null;
        MateriaSeleccionada = null;
        NotaTexto = string.Empty;
        Observacion = string.Empty;

        LimpiarErrores();
    }

    [RelayCommand]
    private void SeleccionarCalificacion(
        Calificacion? calificacion)
    {
        if (calificacion is null)
        {
            return;
        }

        validacionesActivadas = false;

        IdEnEdicion = calificacion.Id;
        EstudianteSeleccionado = calificacion.Estudiante;
        MateriaSeleccionada = calificacion.Materia;
        NotaTexto = calificacion.Nota.ToString(
            CultureInfo.InvariantCulture);
        Observacion = calificacion.Observacion;

        LimpiarErrores();
        EsModoEdicion = true;
    }

    [RelayCommand]
    private async Task EliminarCalificacionActualAsync()
    {
        if (IdEnEdicion is null)
        {
            return;
        }

        var calificaciones =
            await ServicioAcademico.ObtenerCalificacionesAsync();

        var calificacion = calificaciones.FirstOrDefault(
            item => item.Id == IdEnEdicion.Value);

        if (calificacion is null)
        {
            if (SolicitarAlerta is not null)
            {
                await SolicitarAlerta(
                    "Calificación no encontrada",
                    "El registro seleccionado ya no está disponible.",
                    "Aceptar");
            }

            LimpiarFormulario();
            return;
        }

        await ConfirmarYEliminarCalificacionAsync(calificacion);
    }

    [RelayCommand]
    private async Task EliminarCalificacionDesdeListaAsync(
        Calificacion? calificacion)
    {
        if (calificacion is null)
        {
            return;
        }

        await ConfirmarYEliminarCalificacionAsync(calificacion);
    }

    private async Task ConfirmarYEliminarCalificacionAsync(
        Calificacion calificacion)
    {
        if (SolicitarConfirmacion is null)
        {
            return;
        }

        var confirmado = await SolicitarConfirmacion(
            "Eliminar calificación",
            $"¿Seguro que deseas eliminar la calificación de " +
            $"{calificacion.Estudiante} en {calificacion.Materia}?",
            "Eliminar",
            "Cancelar");

        if (!confirmado)
        {
            return;
        }

        var eliminada =
            await ServicioAcademico.EliminarCalificacionAsync(
                calificacion.Id);

        if (!eliminada)
        {
            if (SolicitarAlerta is not null)
            {
                await SolicitarAlerta(
                    "No se pudo eliminar",
                    "La calificación ya no se encuentra disponible.",
                    "Aceptar");
            }

            return;
        }

        if (IdEnEdicion == calificacion.Id)
        {
            LimpiarFormulario();
        }

        await CargarCalificacionesAsync();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                "Calificación eliminada",
                "El registro fue eliminado correctamente.",
                "Aceptar");
        }
    }

    private async Task CargarOpcionesAsync()
    {
        var estudiantes =
            await ServicioAcademico.ObtenerEstudiantesAsync();

        var materias =
            await ServicioAcademico.ObtenerMateriasAsync();

        EstudiantesOpciones.Clear();
        MateriasOpciones.Clear();

        foreach (var estudiante in estudiantes)
        {
            EstudiantesOpciones.Add(estudiante.NombreCompleto);
        }

        foreach (var materia in materias)
        {
            MateriasOpciones.Add(materia.DescripcionCorta);
        }
    }

    private async Task CargarCalificacionesAsync()
    {
        var calificaciones =
            await ServicioAcademico.ObtenerCalificacionesAsync();

        Calificaciones.Clear();

        foreach (var calificacion in calificaciones)
        {
            Calificaciones.Add(calificacion);
        }

        OnPropertyChanged(nameof(TextoCantidadCalificaciones));
    }

    private bool ValidarFormulario()
    {
        LimpiarErrores();

        var esValido = true;

        if (string.IsNullOrWhiteSpace(EstudianteSeleccionado))
        {
            EstudianteError = "Debes seleccionar un estudiante.";
            TieneErrorEstudiante = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(MateriaSeleccionada))
        {
            MateriaError = "Debes seleccionar una materia.";
            TieneErrorMateria = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(NotaTexto))
        {
            NotaError = "La nota es obligatoria.";
            TieneErrorNota = true;
            esValido = false;
        }
        else if (!NotaEsNumeroValido(NotaTexto))
        {
            NotaError = "La nota debe ser un número válido.";
            TieneErrorNota = true;
            esValido = false;
        }
        else
        {
            var nota = ConvertirNota(NotaTexto);

            if (nota < 0 || nota > 100)
            {
                NotaError = "La nota debe estar entre 0 y 100.";
                TieneErrorNota = true;
                esValido = false;
            }
        }

        if (!string.IsNullOrWhiteSpace(Observacion) && Observacion.Trim().Length > 120)
        {
            ObservacionError = "La observación no debe pasar de 120 caracteres.";
            TieneErrorObservacion = true;
            esValido = false;
        }

        if (esValido && ExisteCalificacionRegistrada())
        {
            MateriaError = "Ya existe una calificación para este estudiante y materia.";
            TieneErrorMateria = true;
            esValido = false;
        }

        return esValido;
    }

    private static bool NotaEsNumeroValido(string notaTexto)
    {
        var notaNormalizada = notaTexto.Trim().Replace(',', '.');

        return double.TryParse(
            notaNormalizada,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out _);
    }

    private static double ConvertirNota(string notaTexto)
    {
        var notaNormalizada = notaTexto.Trim().Replace(',', '.');

        return double.Parse(
            notaNormalizada,
            NumberStyles.Number,
            CultureInfo.InvariantCulture);
    }

    private bool ExisteCalificacionRegistrada()
    {
        return Calificaciones.Any(calificacion =>
            (!IdEnEdicion.HasValue ||
             calificacion.Id != IdEnEdicion.Value) &&
            calificacion.Estudiante.Equals(
                EstudianteSeleccionado,
                StringComparison.OrdinalIgnoreCase) &&
            calificacion.Materia.Equals(
                MateriaSeleccionada,
                StringComparison.OrdinalIgnoreCase));
    }

    private void LimpiarErrores()
    {
        EstudianteError = string.Empty;
        MateriaError = string.Empty;
        NotaError = string.Empty;
        ObservacionError = string.Empty;

        TieneErrorEstudiante = false;
        TieneErrorMateria = false;
        TieneErrorNota = false;
        TieneErrorObservacion = false;
    }

    partial void OnEstudianteSeleccionadoChanged(string? value)
    {
        if (!validacionesActivadas && !TieneErrorEstudiante)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            EstudianteError = "Debes seleccionar un estudiante.";
            TieneErrorEstudiante = true;
            return;
        }

        EstudianteError = string.Empty;
        TieneErrorEstudiante = false;
    }

    partial void OnMateriaSeleccionadaChanged(string? value)
    {
        if (!validacionesActivadas && !TieneErrorMateria)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            MateriaError = "Debes seleccionar una materia.";
            TieneErrorMateria = true;
            return;
        }

        MateriaError = string.Empty;
        TieneErrorMateria = false;
    }

    partial void OnNotaTextoChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorNota)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            NotaError = "La nota es obligatoria.";
            TieneErrorNota = true;
            return;
        }

        if (!NotaEsNumeroValido(value))
        {
            NotaError = "La nota debe ser un número válido.";
            TieneErrorNota = true;
            return;
        }

        var nota = ConvertirNota(value);

        if (nota < 0 || nota > 100)
        {
            NotaError = "La nota debe estar entre 0 y 100.";
            TieneErrorNota = true;
            return;
        }

        NotaError = string.Empty;
        TieneErrorNota = false;
    }

    partial void OnObservacionChanged(string value)
    {
        if (!validacionesActivadas && !TieneErrorObservacion)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(value) && value.Trim().Length > 120)
        {
            ObservacionError = "La observación no debe pasar de 120 caracteres.";
            TieneErrorObservacion = true;
            return;
        }

        ObservacionError = string.Empty;
        TieneErrorObservacion = false;
    }
}
