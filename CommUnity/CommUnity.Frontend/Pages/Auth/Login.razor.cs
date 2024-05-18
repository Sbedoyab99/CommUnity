using Blazored.Modal.Services;
using Blazored.Modal;
using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Services;
using CommUnity.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using CommUnity.Shared.Entities;

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
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        private async Task CloseModalAsync()
        {
            wasClose = true;
            await BlazoredModal.CloseAsync(ModalResult.Ok());
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