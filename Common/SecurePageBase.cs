using HsqvLogistica.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace HsqvLogistica.Common
{
    public abstract class SecurePageBase : ComponentBase
    {
        [Inject]
        protected IUserSessionService UserSession { get; set; } = default!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        protected bool IsAuthorized => UserSession.EstaAutenticado;

        protected override async Task OnInitializedAsync()
        {
            await Task.Yield();

            if (!UserSession.EstaAutenticado)
            {
                Navigation.NavigateTo("/login", true);
            }
        }
    }
}