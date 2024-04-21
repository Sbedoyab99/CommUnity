using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;
using System.Net;

namespace CommUnity.FrontEnd.Pages.ResidentialUnits
{
    public partial class ResidentialUnitEdit
    {
        //private ResidentialUnitForm? residentialUnitForm;
        private ResidentialUnitDTO residentialUnitDTO = new();
        private ResidentialUnit? residentialUnit;
        private List<Country>? countries;
        private List<State>? states;
        private List<City>? cities;
        private bool loading = true;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await LoadResidentialUnitAsync();
            await LoadCountriesAsync();
            await LoadStatesAsyn(residentialUnit!.City!.State!.Country!.Id);
            await LoadCitiesAsyn(residentialUnit!.City!.State!.Id);
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
                //ToProductDTO(residentialUnit);

            }
        }

        private async Task LoadCountriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            countries = responseHttp.Response;
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
            loading = false;
        }

        private async Task CountryChangedAsync(ChangeEventArgs e)
        {
            var selectedCountry = Convert.ToInt32(e.Value!);
            states = null;
            cities = null;
            residentialUnit!.CityId = 0;
            await LoadStatesAsyn(selectedCountry);
        }

        private async Task StateChangedAsync(ChangeEventArgs e)
        {
            var selectedState = Convert.ToInt32(e.Value!);
            cities = null;
            residentialUnit!.CityId = 0;
            await LoadCitiesAsyn(selectedState);
        }

        private async Task EditResidentialUnitAsync()
        {

            var responseHttp = await Repository.PutAsync("/api/residentialUnit", ToProductDTO(residentialUnit!));
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
            //residentialUnitForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/residentialUnits");
        }

    }
}