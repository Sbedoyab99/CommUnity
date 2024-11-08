using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Newss
{
    public partial class NewsCreate
    {
        private NewsForm? newsForm;
        private User? _user;
        private bool IsAdmin = false;

        private News news = new();

        [Parameter] public int ResidentialUnitId { get; set; }

        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            news.Date = DateTime.Now;
            await GetUserAsync();
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
            if(_user.UserType == UserType.AdminResidentialUnit)
            {
                IsAdmin = true;
            }
            return;
        }

        private async Task CreateAsync()
        {
            news.ResidentialUnitId = ResidentialUnitId;
            var responseHttp = await Repository.PostAsync("api/news/full", news);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                return;
            }
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "Noticia creada",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }

        private void Return()
        {
            newsForm!.FormPostedSuccesfully = true;
            if (IsAdmin)
            {
                NavigationManager.NavigateTo("/news");
            }
            else
            {
                NavigationManager.NavigateTo($"/news/{ResidentialUnitId}");
            }
        }
    }
}
