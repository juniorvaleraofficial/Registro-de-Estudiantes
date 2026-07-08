using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class StudentFormPage : ContentPage
{
    public StudentFormPage()
    {
        InitializeComponent();

        BindingContext = new StudentFormViewModel();
    }
}