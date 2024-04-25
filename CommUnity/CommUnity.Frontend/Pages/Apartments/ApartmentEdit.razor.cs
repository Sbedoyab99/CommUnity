using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Apartments
{
    public partial class ApartmentEdit
    {
        private Apartment? apartment;
        private ApartmentForm? apartmentForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [EditorRequired, Parameter] public int ApartmentId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Apartment>($"api/apartments/{ApartmentId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                NavigationManager.NavigateTo("/residentialunits");
            }
            else
            {
                apartment = responseHttp.Response!;
            }
        }   

        private async Task EditAsync()
        {
            if(apartment == null)
            {
                return;
            }
            var responseHttp = await Repository.PutAsync("api/apartments", ToApartmentDTO(apartment));
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
                Title = "Apartamento editado",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }

        private ApartmentDTO ToApartmentDTO(Apartment apartment)
        {
            return new ApartmentDTO
            {
                Id = apartment.Id,
                ResidentialUnitId = apartment.ResidentialUnitId,
                Number = apartment.Number
            };
        }

        private void Return()
        {
            apartmentForm!.FormPostedSuccesfully = true;
            NavigationManager.NavigateTo($"/apartments/{apartment?.ResidentialUnitId}");
        }
    }
}