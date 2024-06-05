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

        private int ApartmentId;

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

                var apartmentIdClaim = user.FindFirst(c => c.Type == "ApartmentId");               
                if (apartmentIdClaim != null && int.TryParse(apartmentIdClaim.Value, out int id))
                {
                    ApartmentId = id;
                }

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