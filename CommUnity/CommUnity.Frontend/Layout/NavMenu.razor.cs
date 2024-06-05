using CommUnity.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace CommUnity.FrontEnd.Layout
{
    public partial class NavMenu
    {
        private bool isAdmin;
        private bool isAdminResidentialUnit;
        private bool isResident;

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await CheckIsAuthenticatedAsync();
            AuthenticationStateProvider.AuthenticationStateChanged += AuthStateChanged;
        }

        private async void AuthStateChanged(Task<AuthenticationState> task)
        {
            await CheckIsAuthenticatedAsync();
            StateHasChanged();
        }

        private async Task CheckIsAuthenticatedAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            var user = authenticationState.User;
            if (user.Identity!.IsAuthenticated)
            {
                isAdmin = user.IsInRole("Admin");
                isAdminResidentialUnit = user.IsInRole("AdminResidentialUnit");
                isResident = user.IsInRole("Resident");
            }
            else
            {
                isAdmin = false;
                isAdminResidentialUnit = false;
                isResident = false;
            }
        }
    }
}