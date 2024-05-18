using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages
{
    public partial class Home
    {
        private int totalPages = 0;
        private List<News>? newsList;
        private bool loading = true;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadPagesAsync();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            loading = true;
            var ok = await LoadListAsync();
            if (ok)
            {
                loading = false;
            }
        }

        private async Task<bool> LoadListAsync(int page = 1)
        {
            string url = $"/api/news?page={page}&recordsnumber=5";

            var responseHttp = await Repository.GetAsync<List<News>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return false;
            }
            newsList = responseHttp.Response;
            return true;
        }

        private async Task<bool> LoadPagesAsync(int ResidentialUnitId = 0)
        {
            string url = $"/api/news/totalPages?recordsnumber=5";
            if (ResidentialUnitId != 0)
            {
                url += $"&id={ResidentialUnitId}";
            }
            var responseHttp = await Repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return false;
            }
            totalPages = responseHttp.Response;
            return true;
        }

        private static string TruncateContent(string content, int length)
        {
            if (content.Length > length)
            {
                return content.Substring(0, length) + "...";
            }
            return content;
        }

        private void LogInAction()
        {
            NavigationManager.NavigateTo("/soon");
        }
    }
}
