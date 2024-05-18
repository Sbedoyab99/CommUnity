using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Services;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Auth
{
    public partial class Logout
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        private async Task LogoutAction()
        {
            await LoginService.LogoutAsync();
            NavigationManager.NavigateTo("/");
        }

        private async Task CancelAction()
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }
    }
}