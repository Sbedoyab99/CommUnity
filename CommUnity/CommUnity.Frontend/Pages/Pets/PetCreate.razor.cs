using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Pets
{
    public partial class PetCreate
    {
        private PetForm?petForm;
        private Pet pet = new();

        [Parameter] public int ApartmentId { get; set; }

        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            pet.ApartmentId = ApartmentId;
            var responseHttp = await Repository.PostAsync("api/pets", pet);
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
                Title = "Mascota  creada",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }
        private void Return()
        {
            petForm!.FormPostedSuccesfully = true;
            NavigationManager.NavigateTo($"/pets/{ApartmentId}");
        }
    }
}
