using RegistroEstudiantes.Mobile.ViewModels;

namespace RegistroEstudiantes.Mobile.Views;

public partial class PerfilAcademicoPage :
    ContentPage,
    IQueryAttributable
{
    private readonly PerfilAcademicoViewModel viewModel;
    private int estudianteId;

    public PerfilAcademicoPage()
    {
        InitializeComponent();

        viewModel = new PerfilAcademicoViewModel();
        BindingContext = viewModel;
    }

    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (!query.TryGetValue("id", out var valorId))
        {
            estudianteId = 0;
            return;
        }

        var textoId = Uri.UnescapeDataString(
            valorId?.ToString() ?? string.Empty);

        estudianteId = int.TryParse(textoId, out var id)
            ? id
            : 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (estudianteId <= 0)
        {
            await MostrarErrorYRegresarAsync();
            return;
        }

        var encontrado =
            await viewModel.CargarPerfilAsync(estudianteId);

        if (!encontrado)
        {
            await MostrarErrorYRegresarAsync();
        }
    }

    private static async Task MostrarErrorYRegresarAsync()
    {
        await Shell.Current.DisplayAlertAsync(
            "Perfil no disponible",
            "No fue posible encontrar el estudiante seleccionado.",
            "Aceptar");

        await Shell.Current.GoToAsync("..");
    }
}