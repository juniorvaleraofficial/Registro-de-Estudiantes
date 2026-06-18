using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile.Views;

public partial class StudentsPage : ContentPage
{
    private readonly StudentApiService _studentApiService;

    public StudentsPage()
    {
        InitializeComponent();

        _studentApiService = new StudentApiService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadStudentsAsync();
    }

    private async Task LoadStudentsAsync()
    {
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        StatusLabel.Text = "Cargando estudiantes desde la API...";

        var students = await _studentApiService.GetStudentsAsync();

        StudentsCollectionView.ItemsSource = students;

        StatusLabel.Text = students.Count == 0
            ? "No se encontraron estudiantes o la API no está disponible."
            : $"Estudiantes cargados: {students.Count}";

        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await LoadStudentsAsync();
    }

    private async void OnCreateStudentClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//student-form");
    }
}