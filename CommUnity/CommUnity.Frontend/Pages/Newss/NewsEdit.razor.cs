using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Newss
{
    public partial class NewsEdit
    {
        private NewsDTO newsDTO = new();
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
                newsDTO = ToNewsDTO(responseHttp.Response!);
            }
        }

        private async Task EditAsync()
        {
            if (newsDTO == null)
            {
                return;
            }
            var responseHttp = await Repository.PutAsync("api/news/full", newsDTO);
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

        private NewsDTO ToNewsDTO(News news)
        {
            return new NewsDTO
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
            NavigationManager.NavigateTo($"/news/{newsDTO?.ResidentialUnitId}");
        }
    }
}