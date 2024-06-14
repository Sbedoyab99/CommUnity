using CommUnity.FrontEnd.Pages.ResidentialUnits;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Cities
{
    public partial class ResidentialUnitEditWithCity
    {
        private ResidentialUnitFormWithCity? residentialUnitFormWithCity;
        private bool loading = true;

        private ResidentialUnit residentialUnit = new();

        [Parameter] public int ResidentialUnitId { get; set; }
        [Parameter] public int CityId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            await LoadResidentialUnitAsync();
        }

        private async Task LoadResidentialUnitAsync()
        {
            var responseHttp = await Repository.GetAsync<ResidentialUnit>($"api/residentialUnit/{ResidentialUnitId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
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
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            MudDialog.Close(DialogResult.Ok(true));
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con exito.");
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
            residentialUnitFormWithCity!.FormPostedSuccessfully = true;
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}