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

    public event Func<string, string, string, Task>? SolicitarAlerta;

    private bool validacionesActivadas;

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

        var asistencia = new Asistencia
        {
            Estudiante = EstudianteSeleccionado?.Trim() ?? string.Empty,
            Materia = MateriaSeleccionada?.Trim() ?? string.Empty,
            Fecha = Fecha,
            Estado = EstadoSeleccionado?.Trim() ?? string.Empty
        };

        ServicioAcademico.AgregarAsistencia(asistencia);

        CargarAsistencias();
        LimpiarFormulario();

        if (SolicitarAlerta is not null)
        {
            await SolicitarAlerta(
                "Asistencia guardada",
                "La asistencia fue registrada correctamente en memoria.",
                "Aceptar");
        }
    }

    [RelayCommand]
    private void LimpiarFormulario()
    {
        validacionesActivadas = false;

        EstudianteSeleccionado = null;
        MateriaSeleccionada = null;
        Fecha = DateTime.Today;
        EstadoSeleccionado = null;

        LimpiarErrores();
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
                asistencia.Estudiante.Equals(EstudianteSeleccionado, StringComparison.OrdinalIgnoreCase) &&
                asistencia.Materia.Equals(MateriaSeleccionada, StringComparison.OrdinalIgnoreCase) &&
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