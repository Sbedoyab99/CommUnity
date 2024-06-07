using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Services;
using CommUnity.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Auth
{
    public partial class Login
    {
        private LoginDTO loginDTO = new();

        private bool wasClose;

        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

        private void CloseModalAsync()
        {
            wasClose = true;
            MudDialog.Cancel();
        }

        private async Task LoginAsync()
        {
            if (wasClose)
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            var responseHttp = await Repository.PostAsync<LoginDTO, TokenDTO>("/api/accounts/Login", loginDTO);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await LoginService.LoginAsync(responseHttp.Response!.Token);
            NavigationManager.NavigateTo("/");
        }
    }
}