using System.Net.Http.Json;
using RegistroEstudiantes.Mobile.Models;

namespace RegistroEstudiantes.Mobile.Services;

public class StudentApiService
{
    private readonly HttpClient _httpClient;

    public StudentApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(GetApiBaseUrl())
        };
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        try
        {
            var students = await _httpClient.GetFromJsonAsync<List<Student>>("api/students");

            return students ?? new List<Student>();
        }
        catch
        {
            return new List<Student>();
        }
    }

    private static string GetApiBaseUrl()
    {
#if ANDROID
        return "http://10.0.2.2:5162/";
#else
        return "http://localhost:5162/";
#endif
    }
}