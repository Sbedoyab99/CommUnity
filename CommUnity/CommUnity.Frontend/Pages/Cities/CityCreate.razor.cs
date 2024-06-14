using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Shared;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Cities
{
    public partial class CityCreate
    {
        private City city = new();
        private FormWithName<City>? cityForm;

        [Parameter] public int StateId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

        private async Task CreateAsync()
        {
            city.StateId = StateId;
            var responseHttp = await Repository.PostAsync("api/cities", city);
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
            await toast.FireAsync("Registro creado con �xito.", "", SweetAlertIcon.Success);
        }

        private void Return()
        {
            cityForm!.FormPostedSuccesfully = true;
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}