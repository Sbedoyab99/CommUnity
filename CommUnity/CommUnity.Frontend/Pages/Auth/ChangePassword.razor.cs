using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Auth
{
    public partial class ChangePassword
    {
        private ChangePasswordDTO changePasswordDTO = new();
        private bool loading;

        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private async Task ChangePasswordAsync()
        {
            loading = true;
            var responseHttp = await Repository.PostAsync("/api/accounts/changePassword", changePasswordDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            MudDialog.Close(DialogResult.Ok(true));
            await SweetAlertService.FireAsync("Confirmaci�n", "Contrase�a Modificada con �xito.", SweetAlertIcon.Info);
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
        }

        private void ReturnAction()
        {
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}