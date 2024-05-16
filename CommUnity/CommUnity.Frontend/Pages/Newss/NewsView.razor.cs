using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Shared;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Newss
{
    public partial class NewsView
    {
        private News? news;
        private bool loading = true;

        [Parameter] public int NewsId { get; set; }
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadNewsAsync();
        }

        private async Task<bool> LoadNewsAsync()
        {
            loading = true;
            var responseHttp = await Repository.GetAsync<News>($"/api/news/{NewsId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    loading = false;
                    return false;
                }

                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                loading = false;
                return false;
            }
            news = responseHttp.Response;
            loading = false;
            return true;
        }

        private IEnumerable<string> GetParagraphs(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return Enumerable.Empty<string>();
            }

            return content.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.None);
        }

        private void ReturnAction()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}