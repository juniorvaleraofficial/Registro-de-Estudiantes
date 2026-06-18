namespace RegistroEstudiantes.Mobile.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnStudentsClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//students");
    }

    private async void OnCreateStudentClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//student-form");
    }
}