using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel viewModel;

    public LoginPage()
    {
        InitializeComponent();

        viewModel = new LoginViewModel();
        BindingContext = viewModel;

        viewModel.LoginExitoso += async () =>
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            await Shell.Current.GoToAsync("//home");
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        viewModel.LimpiarFormularioCommand.Execute(null);
    }
}