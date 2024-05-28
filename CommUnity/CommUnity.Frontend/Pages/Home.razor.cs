using Blazored.Modal.Services;
using CommUnity.FrontEnd.Pages.Auth;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages
{
    public partial class Home
    {
        private int totalPages = 0;
        private List<News>? newsList;
        private bool loading = true;
        private User user = null!;
        private bool isAuthenticated;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        [CascadingParameter] private IModalService Modal { get; set; } = default!;

        protected override void OnInitialized()
        {
            AuthenticationStateProvider.AuthenticationStateChanged += AuthStateChanged;
        }

        private async void AuthStateChanged(Task<AuthenticationState> task)
        {
            await CheckIsAuthenticatedAsync();
            StateHasChanged();
        }

        protected async override Task OnParametersSetAsync()
        {
            await CheckIsAuthenticatedAsync();
            var ok = await LoadUserAsyc();
            if (ok)
            {
                await LoadPagesAsync(user.ResidentialUnitId);
                await LoadAsync();
            } else
            {
                await LoadPagesAsync();
                await LoadAsync();
            }
        }

        private async Task CheckIsAuthenticatedAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            isAuthenticated = authenticationState.User.Identity!.IsAuthenticated;
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

        private async Task<bool> LoadUserAsyc()
        {
            user = null!;
            if (!isAuthenticated)
            {
                return false;
            }
            var responseHttp = await Repository.GetAsync<User>($"/api/accounts");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                var messageError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return false;
            }
            user = responseHttp.Response!;
            return true;
        }

        private async Task<bool> LoadListAsync(int page = 1)
        {
            string url;
            if (user != null)
            {
                if (user.ResidentialUnitId != null)
                {
                    url = $"/api/news?id={user.ResidentialUnitId}&page={page}&recordsnumber=5";
                }
                else
                {
                    url = $"/api/news?page={page}&recordsnumber=5";
                }
            } else
            {
                url = $"/api/news?page={page}&recordsnumber=5";
            }
            
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

        private async Task<bool> LoadPagesAsync(int? ResidentialUnitId = 0)
        {
            string url;
            if (user != null)
            {
                if(user.ResidentialUnitId != null)
                {
                    url = $"/api/news/totalPages?id={user.ResidentialUnitId}&recordsnumber=5";
                }
                else
                {
                    url = $"/api/news/totalPages?recordsnumber=5";
                }
            }
            else
            {
                url = $"/api/news/totalPages?recordsnumber=5";
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
        private void ShowModalLogIn()
        {
            Modal.Show<Login>();
        }
    }
}
