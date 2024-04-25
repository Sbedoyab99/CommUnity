using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Newss
{
    public partial class NewsCreate
    {
        private NewsForm? newsForm;
        private News news = new();

        [Parameter] public int ResidentialUnitId { get; set; }

        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override void OnInitialized()
        {
            news.Date = DateTime.Now;
        }

        private async Task CreateAsync()
        {
            news.ResidentialUnitId = ResidentialUnitId;
            var responseHttp = await Repository.PostAsync("api/news", news);
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
            NavigationManager.NavigateTo($"/news/{ResidentialUnitId}");
        }
    }
}
