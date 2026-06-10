namespace RegistroEstudiantes.Mobile.Views;

public partial class StudentsPage : ContentPage
{
    public StudentsPage()
    {
        InitializeComponent();
    }

    private async void OnCreateStudentClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//student-form");
    }
}