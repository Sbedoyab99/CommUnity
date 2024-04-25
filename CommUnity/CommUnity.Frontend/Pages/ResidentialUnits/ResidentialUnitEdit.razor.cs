using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace CommUnity.FrontEnd.Pages.ResidentialUnits
{
    public partial class ResidentialUnitEdit
    {
        ResidentialUnitForm? residentialUnitForm;
        private bool loading = true;

        ResidentialUnit residentialUnit = new();

        [Parameter] public int Id { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            await LoadResidentialUnitAsync();
        }

        private async Task LoadResidentialUnitAsync()
        {
            var responseHttp = await Repository.GetAsync<ResidentialUnit>($"api/residentialUnit/{Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/residentialUnits");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "Error",
                        Text = message,
                        Icon = SweetAlertIcon.Error,
                    });
                }
            }
            else
            {
                residentialUnit = responseHttp.Response!;
                loading = false;
            }
        }

        private async Task EditResidentialUnitAsync()
        {

            var responseHttp = await Repository.PutAsync("/api/residentialUnit", ToResidentialUnitDTO(residentialUnit));
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con éxito.");
        }

        private ResidentialUnitDTO ToResidentialUnitDTO(ResidentialUnit residentialUnit)
        {
            return new ResidentialUnitDTO
            {
                Id = residentialUnit.Id,
                Name = residentialUnit.Name,
                Address = residentialUnit.Address,
                CityId = residentialUnit.CityId
            };
        }

        private void Return()
        {
            residentialUnitForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/residentialUnits");
        }

    }
}