using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace CommUnity.FrontEnd.Pages.ResidentialUnits
{
    public partial class ResidentialEdit
    {
        ResidentialForm? residentialForm;
        private bool loading = true;

        //ResidentialUnitDTO residentialUnitDTO = new();
        ResidentialUnit residentialUnit = new();

        private List<State>? states;
        private List<City>? cities;

        [Parameter] public int Id { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            await LoadResidentialUnitAsync();
            //await LoadCountriesAsync();
            //await LoadStatesAsyn(residentialUnit!.City!.State!.Country!.Id);
            //await LoadCitiesAsyn(residentialUnit!.City!.State!.Id);
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
                //residentialUnitDTO = ToProductDTO(residentialUnit);
                loading = false;
            }
        }

        private async Task LoadStatesAsyn(int countryId)
        {
            var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            states = responseHttp.Response;
        }

        private async Task LoadCitiesAsyn(int stateId)
        {
            var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            cities = responseHttp.Response;
        }

        private async Task EditResidentialUnitAsync()
        {

            var responseHttp = await Repository.PutAsync("/api/residentialUnit", ToProductDTO(residentialUnit));
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

        private ResidentialUnitDTO ToProductDTO(ResidentialUnit residentialUnit)
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
            residentialForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/residentialUnits");
        }

    }
}