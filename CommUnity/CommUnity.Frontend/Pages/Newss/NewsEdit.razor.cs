using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Newss
{
    public partial class NewsEdit
    {
        private News? news;
        private NewsForm? newsForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [EditorRequired, Parameter] public int NewsId { get; set; }

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
                news = responseHttp.Response!;
            }
        }   

        private async Task EditAsync()
        {
            if(news == null)
            {
                return;
            }
            var responseHttp = await Repository.PutAsync("api/news", ToNewsDTO(news));
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
                Title = "Noticias editada",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }

        private NewsDTO ToNewsDTO(News news)
        {
            return new NewsDTO
            {
                Id = news.Id,
                ResidentialUnitId = news.ResidentialUnitId,
                Title = news.Title
            };
        }

        private void Return()
        {
           newsForm!.FormPostedSuccesfully = true;
           NavigationManager.NavigateTo($"/news/{news?.ResidentialUnitId}");
        }
    }
}