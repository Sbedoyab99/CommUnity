using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.MyResidentialUnit
{
    public partial class Events
    {
        private User _user = null!;
        private bool loading;

        private int ResidentialUnitId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await GetUserAsync();
        }

        private async Task GetUserAsync()
        {
            loading = true;
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
            loading = false;
            return;
        }
    }
}