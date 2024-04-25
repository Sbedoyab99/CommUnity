using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Apartments
{
    public partial class ApartmentCreate
    {
        private ApartmentForm? apartmentForm;
        private Apartment apartment = new();

        [Parameter] public int ResidentialUnitId { get; set; }

        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            apartment.ResidentialUnitId = ResidentialUnitId;
            var responseHttp = await Repository.PostAsync("api/apartments", apartment);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                return;
            }
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "Apartamento creado",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }

        private void Return()
        {
            apartmentForm!.FormPostedSuccesfully = true;
            NavigationManager.NavigateTo($"/apartments/{ResidentialUnitId}");
        }
    }
}