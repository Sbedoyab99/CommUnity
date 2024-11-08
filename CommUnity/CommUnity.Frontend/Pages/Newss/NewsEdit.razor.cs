using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Newss
{
    public partial class NewsEdit
    {
        private News news = new();
        private NewsForm? newsForm;

        private User? _user;
        private bool IsAdmin = false;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [EditorRequired, Parameter] public int NewsId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUserAsync();
            if (_user == null)
            {
                NavigationManager.NavigateTo("/");
                return;
            }
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
            if (_user.UserType == UserType.AdminResidentialUnit)
            {
                IsAdmin = true;
            }
            return;
        }

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<News>($"api/news/{NewsId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                NavigationManager.NavigateTo("/residentialunits");
            }
            else
            {
                news = ToNewsDTO(responseHttp.Response!);
            }
        }

        private async Task EditAsync()
        {
            if (news == null)
            {
                return;
            }
            var responseHttp = await Repository.PutAsync("api/news/full", news);
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
                Title = "Noticia editada",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }

        private News ToNewsDTO(News news)
        {
            return new News
            {
                Id = news.Id,
                ResidentialUnitId = news.ResidentialUnitId,
                Title = news.Title,
                Content = news.Content,
                Date = news.Date,
                Picture = news.Picture
            };
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
                NavigationManager.NavigateTo($"/news/{news?.ResidentialUnitId}");
            }
        }
    }
}