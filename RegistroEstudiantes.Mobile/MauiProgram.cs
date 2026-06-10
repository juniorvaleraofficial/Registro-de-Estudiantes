using Microsoft.Extensions.Logging;
using RegistroEstudiantes.Mobile.Services;

namespace RegistroEstudiantes.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        builder.Services.AddSingleton<StudentApiService>();
#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
	}
}
