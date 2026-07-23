using HsqvLogistica.Models.DTOs.Usuarios;
using HsqvLogistica.Services.Interfaces;
using Microsoft.JSInterop;

public class LoginStorageService : ILoginStorageService
{
    private readonly IJSRuntime _js;

    public LoginStorageService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task GuardarAsync(LoginRememberDto model)
    {
        try {
            await _js.InvokeVoidAsync(
            "loginStorage.save",
            model);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar en localStorage: {ex.Message}");
            throw;
        }
    }

    public async Task<LoginRememberDto?> ObtenerAsync()
    {
        var model = await _js.InvokeAsync<LoginRememberDto?>(
            "loginStorage.get");

        if (model == null)
            return null;

        if (model.Expiration < DateTime.Now)
        {
            await EliminarAsync();
            return null;
        }

        return model;
    }

    public async Task EliminarAsync()
    {
        await _js.InvokeVoidAsync(
            "loginStorage.remove");
    }
}