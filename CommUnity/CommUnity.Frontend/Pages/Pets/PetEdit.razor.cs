using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Pets
{
    public partial class PetEdit
    {
        private Pet? pet;
        private PetForm? petForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [EditorRequired, Parameter] public int PetId { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Pet>($"api/pets/{PetId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                NavigationManager.NavigateTo("/apartments");
            }
            else
            {
                pet = responseHttp.Response!;
            }
        }   

        private async Task EditAsync()
        {
            if(pet == null)
            {
                return;
            }
            var responseHttp = await Repository.PutAsync("api/pets", ToPetDTO(pet));
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
                Title = "Mascota editada",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }

        private PetDTO ToPetDTO(Pet pet)
        {
            return new PetDTO
            {
                Id = pet.Id,
                ApartmentId = pet.ApartmentId,
                Name = pet.Name,    
                Breed = pet.Breed

            };
        }

        private void Return()
        {
           petForm!.FormPostedSuccesfully = true;
           NavigationManager.NavigateTo($"/pets/{pet?.ApartmentId}");
        }
    }
}