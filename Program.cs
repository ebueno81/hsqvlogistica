using HsqvLogistica.Components;
using HsqvLogistica.Data;
using HsqvLogistica.Integrations.Auth;
using HsqvLogistica.Integrations.Clients;
using HsqvLogistica.Integrations.Clients.Interfaces;
using HsqvLogistica.Repositories;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services;
using HsqvLogistica.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor Components (.NET 8)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor
builder.Services.AddMudServices();

// Configuración
builder.Configuration.AddJsonFile("appsettings.json");

// DbContext
builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));
});

// DI
builder.Services.AddScoped<IAlmacenRepository, AlmacenRepository>();
builder.Services.AddScoped<IAlmacenService, AlmacenService>();
builder.Services.AddScoped<ILineaRepository, LineaRepository>();
builder.Services.AddScoped<ILineaService, LineaService>();
builder.Services.AddScoped<IArticuloRepository, ArticuloRepository>();
builder.Services.AddScoped<IArticuloService, ArticuloService>();
builder.Services.AddScoped<IMotivoRepository, MotivoRepository>();
builder.Services.AddScoped<IMotivoService, MotivoService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

// Controllers (API)
builder.Services.AddControllers();

builder.Services.AddSingleton<AuthTokenStore>();
builder.Services.AddScoped<AuthTokenHandler>();
builder.Services.AddScoped<RemoteLoginService>();

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalApis:SistemaPedidos:BaseUrl"]!
    );
});

// 🔐 Cliente API protegido
builder.Services.AddHttpClient<IClienteApiClient, ClienteApiClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalApis:SistemaPedidos:BaseUrl"]!
    );
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<IEmpServApiClient, EmpServApiClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalApis:SistemaPedidos:BaseUrl"]!
    );
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<IEmpresaApiClient, EmpresaApiClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalApis:SistemaPedidos:BaseUrl"]!
    );
})
.AddHttpMessageHandler<AuthTokenHandler>();

// HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var loginService = scope.ServiceProvider
        .GetRequiredService<RemoteLoginService>();

    await loginService.EnsureTokenAsync();
}

// Controllers
app.MapControllers();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// 🔴 NO MapGet("/") aquí

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
