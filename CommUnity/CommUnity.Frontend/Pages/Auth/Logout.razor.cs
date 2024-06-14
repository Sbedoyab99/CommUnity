using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Auth
{
    public partial class Logout
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private async Task LogoutAction()
        {
            await LoginService.LogoutAsync();
            NavigationManager.NavigateTo("/");
        }

        private void CancelAction()
        {
            MudDialog.Cancel();
        }
    }
}