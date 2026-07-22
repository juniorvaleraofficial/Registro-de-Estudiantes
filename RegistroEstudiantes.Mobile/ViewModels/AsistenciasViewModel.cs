using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RegistroEstudiantes.Mobile.Models;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.ViewModels;

public partial class AsistenciasViewModel : ObservableObject
{
    public ObservableCollection<string> EstudiantesOpciones { get; } = new();
    public ObservableCollection<string> MateriasOpciones { get; } = new();
    public ObservableCollection<string> EstadosOpciones { get; } = new();
    public ObservableCollection<Asistencia> Asistencias { get; } = new();

    public string TextoCantidadAsistencias =>
        Asistencias.Count == 1
            ? "1 registro"
            : $"{Asistencias.Count} registros";

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
    private Guid? idEnEdicion;

    public string TituloFormulario =>
        EsModoEdicion ? "Editar asistencia" : "Nueva asistencia";

    public string TextoBotonGuardar =>
        EsModoEdicion ? "Guardar cambios" : "Guardar asistencia";

    public string TextoBotonSecundario =>
        EsModoEdicion ? "Cancelar edición" : "Limpiar formulario";

    public bool MostrarBotonEliminar => EsModoEdicion;

    [ObservableProperty]
    private string? estudianteSeleccionado;

    [ObservableProperty]
    private string? materiaSeleccionada;

    [ObservableProperty]
    private DateTime fecha = DateTime.Today;

    [ObservableProperty]
    private string? estadoSeleccionado;

    [ObservableProperty]
    private string estudianteError = string.Empty;

    [ObservableProperty]
    private string materiaError = string.Empty;

    [ObservableProperty]
    private string fechaError = string.Empty;

    [ObservableProperty]
    private string estadoError = string.Empty;

    [ObservableProperty]
    private bool tieneErrorEstudiante;

    [ObservableProperty]
    private bool tieneErrorMateria;

    [ObservableProperty]
    private bool tieneErrorFecha;

    [ObservableProperty]
    private bool tieneErrorEstado;

    public AsistenciasViewModel()
    {
        ServicioAcademico.CargarDatosDePrueba();

        CargarOpciones();
        CargarAsistencias();
    }

    [RelayCommand]
    private async Task GuardarAsistenciaAsync()
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
                    "No se encontró el identificador de la asistencia seleccionada.",
                    "Aceptar");
            }

            return;
        }

        var asistencia = new Asistencia
        {
            Id = IdEnEdicion ?? Guid.NewGuid(),
            Estudiante =
                EstudianteSeleccionado?.Trim() ?? string.Empty,
            Materia =
                MateriaSeleccionada?.Trim() ?? string.Empty,
            Fecha = Fecha.Date,
            Estado =
                EstadoSeleccionado?.Trim() ?? string.Empty
        };

        if (estabaEditando)
        {
            var actualizada =
                ServicioAcademico.ActualizarAsistencia(asistencia);

            if (!actualizada)
            {
                if (SolicitarAlerta is not null)
                {
                    await SolicitarAlerta(
                        "No se pudo actualizar",
                        "La asistencia seleccionada ya no se encuentra disponible.",
                        "Aceptar");
                }

                return;
            }
        }
        else
        {
            ServicioAcademico.AgregarAsistencia(asistencia);
        }

        CargarAsistencias();
        LimpiarFormulario();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                estabaEditando
                    ? "Asistencia actualizada"
                    : "Asistencia guardada",
                estabaEditando
                    ? "Los cambios de la asistencia se guardaron correctamente."
                    : "La asistencia fue registrada correctamente en memoria.",
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
        Fecha = DateTime.Today;
        EstadoSeleccionado = null;

        LimpiarErrores();
    }

    [RelayCommand]
    private void SeleccionarAsistencia(Asistencia? asistencia)
    {
        if (asistencia is null)
        {
            return;
        }

        validacionesActivadas = false;

        IdEnEdicion = asistencia.Id;
        EstudianteSeleccionado = asistencia.Estudiante;
        MateriaSeleccionada = asistencia.Materia;
        Fecha = asistencia.Fecha;
        EstadoSeleccionado = asistencia.Estado;

        LimpiarErrores();
        EsModoEdicion = true;
    }

    [RelayCommand]
    private async Task EliminarAsistenciaActualAsync()
    {
        if (IdEnEdicion is null)
        {
            return;
        }

        var asistencia = ServicioAcademico
            .ObtenerAsistencias()
            .FirstOrDefault(item => item.Id == IdEnEdicion.Value);

        if (asistencia is null)
        {
            if (SolicitarAlerta is not null)
            {
                await SolicitarAlerta(
                    "Asistencia no encontrada",
                    "El registro seleccionado ya no está disponible.",
                    "Aceptar");
            }

            LimpiarFormulario();
            return;
        }

        await ConfirmarYEliminarAsistenciaAsync(asistencia);
    }

    [RelayCommand]
    private async Task EliminarAsistenciaDesdeListaAsync(
        Asistencia? asistencia)
    {
        if (asistencia is null)
        {
            return;
        }

        await ConfirmarYEliminarAsistenciaAsync(asistencia);
    }

    private async Task ConfirmarYEliminarAsistenciaAsync(
        Asistencia asistencia)
    {
        if (SolicitarConfirmacion is null)
        {
            return;
        }

        var confirmado = await SolicitarConfirmacion(
            "Eliminar asistencia",
            $"¿Seguro que deseas eliminar la asistencia de " +
            $"{asistencia.Estudiante} en {asistencia.Materia}?",
            "Eliminar",
            "Cancelar");

        if (!confirmado)
        {
            return;
        }

        var eliminada =
            ServicioAcademico.EliminarAsistencia(asistencia.Id);

        if (!eliminada)
        {
            if (SolicitarAlerta is not null)
            {
                await SolicitarAlerta(
                    "No se pudo eliminar",
                    "La asistencia ya no se encuentra disponible.",
                    "Aceptar");
            }

            return;
        }

        if (IdEnEdicion == asistencia.Id)
        {
            LimpiarFormulario();
        }

        CargarAsistencias();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                "Asistencia eliminada",
                "El registro fue eliminado correctamente.",
                "Aceptar");
        }
    }

    private void CargarOpciones()
    {
        EstudiantesOpciones.Clear();
        MateriasOpciones.Clear();
        EstadosOpciones.Clear();

        foreach (var estudiante in ServicioAcademico.ObtenerEstudiantes())
        {
            EstudiantesOpciones.Add(estudiante.NombreCompleto);
        }

        foreach (var materia in ServicioAcademico.ObtenerMaterias())
        {
            MateriasOpciones.Add(materia.DescripcionCorta);
        }

        EstadosOpciones.Add("Presente");
        EstadosOpciones.Add("Ausente");
        EstadosOpciones.Add("Excusa");
    }

    private void CargarAsistencias()
    {
        Asistencias.Clear();

        foreach (var asistencia in ServicioAcademico.ObtenerAsistencias())
        {
            Asistencias.Add(asistencia);
        }

        OnPropertyChanged(nameof(TextoCantidadAsistencias));
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

        if (Fecha > DateTime.Today)
        {
            FechaError = "La fecha no puede ser futura.";
            TieneErrorFecha = true;
            esValido = false;
        }

        if (string.IsNullOrWhiteSpace(EstadoSeleccionado))
        {
            EstadoError = "Debes seleccionar el estado de asistencia.";
            TieneErrorEstado = true;
            esValido = false;
        }

        if (esValido && ExisteAsistenciaRegistrada())
        {
            EstadoError = "Ya existe una asistencia para este estudiante, materia y fecha.";
            TieneErrorEstado = true;
            esValido = false;
        }

        return esValido;
    }

    private bool ExisteAsistenciaRegistrada()
    {
        return ServicioAcademico
            .ObtenerAsistencias()
            .Any(asistencia =>
                (!IdEnEdicion.HasValue ||
                 asistencia.Id != IdEnEdicion.Value) &&
                asistencia.Estudiante.Equals(
                    EstudianteSeleccionado,
                    StringComparison.OrdinalIgnoreCase) &&
                asistencia.Materia.Equals(
                    MateriaSeleccionada,
                    StringComparison.OrdinalIgnoreCase) &&
                asistencia.Fecha.Date == Fecha.Date);
    }

    private void LimpiarErrores()
    {
        EstudianteError = string.Empty;
        MateriaError = string.Empty;
        FechaError = string.Empty;
        EstadoError = string.Empty;

        TieneErrorEstudiante = false;
        TieneErrorMateria = false;
        TieneErrorFecha = false;
        TieneErrorEstado = false;
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

    partial void OnFechaChanged(DateTime value)
    {
        if (!validacionesActivadas && !TieneErrorFecha)
        {
            return;
        }

        if (value > DateTime.Today)
        {
            FechaError = "La fecha no puede ser futura.";
            TieneErrorFecha = true;
            return;
        }

        FechaError = string.Empty;
        TieneErrorFecha = false;
    }

    partial void OnEstadoSeleccionadoChanged(string? value)
    {
        if (!validacionesActivadas && !TieneErrorEstado)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            EstadoError = "Debes seleccionar el estado de asistencia.";
            TieneErrorEstado = true;
            return;
        }

        EstadoError = string.Empty;
        TieneErrorEstado = false;
    }
}