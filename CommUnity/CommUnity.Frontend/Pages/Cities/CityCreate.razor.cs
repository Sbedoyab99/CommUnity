using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Shared;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Cities
{
    public partial class CityCreate
    {
        private City city = new();
        private FormWithName<City>? cityForm;

        [Parameter] public int StateId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        
        private async Task CreateAsync()
        {
            city.StateId = StateId;
            var responseHttp = await Repository.PostAsync("api/cities", city);
            if(responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync("Registro creado con éxito.", "", SweetAlertIcon.Success);
        }

        private void Return()
        {
            cityForm!.FormPostedSuccesfully = true;
            NavigationManager.NavigateTo($"/states/details/{StateId}");
        }
    }
}