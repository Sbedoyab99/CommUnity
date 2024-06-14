using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Shared;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.States
{
    public partial class StateCreate
    {
        private State state = new();
        private FormWithName<State>? stateForm;

        [Parameter] public int CountryId { get; set; }
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private async Task CreateAsync()
        {
            state.CountryId = CountryId;
            var responseHttp = await Repository.PostAsync("api/states", state);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            MudDialog.Close(DialogResult.Ok(true));
            await toast.FireAsync("Registro creado con exito.", "", SweetAlertIcon.Success);
        }

        private void Return()
        {
            stateForm!.FormPostedSuccesfully = true;
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}