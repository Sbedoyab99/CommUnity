using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;

namespace CommUnity.FrontEnd.Layout
{
    public partial class NavMenu
    {
        private bool isAdmin;
        private bool isAdminResidentialUnit;
        private bool isResident;
        private bool isWorker;
        private bool loading = true;

        private User _user = null!;
        private int ApartmentId;

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        protected override void OnInitialized()
        {
            AuthenticationStateProvider.AuthenticationStateChanged += AuthStateChanged;
        }

        protected override async Task OnParametersSetAsync()
        {
            await CheckIsAuthenticatedAsync();
            await GetUserAsync();
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
            Console.WriteLine(user.Claims);
            if (user.Identity!.IsAuthenticated)
            {
                isAdmin = user.IsInRole("Admin");
                isAdminResidentialUnit = user.IsInRole("AdminResidentialUnit");
                isResident = user.IsInRole("Resident");
                isWorker = user.IsInRole("Worker");

                if (isResident)
                {
                    var apartmentIdClaim = user.FindFirst(c => c.Type == "ApartmentId");
                    if (apartmentIdClaim != null && int.TryParse(apartmentIdClaim.Value, out int id))
                    {
                        ApartmentId = id;
                    }
                }

                if(isWorker || isAdminResidentialUnit)
                {
                    await GetUserAsync();                  
                }               
            }
            else
            {
                isAdmin = false;
                isAdminResidentialUnit = false;
                isResident = false;
                isWorker = false;
            }
            loading = false;
        }

        private async Task GetUserAsync()
        {
            var responseHttp = await Repository.GetAsync<User>($"/api/accounts");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }
                return;
            }
            _user = responseHttp.Response!;
            return;
        }
    }
}