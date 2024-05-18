using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace CommUnity.FrontEnd.Pages.ResidentialUnits
{
    public partial class ResidentialUnitForm
    {
        private EditContext editContext = null!;
        private bool loading;

        private List<Country>? countries;
        private List<State>? states;
        private List<City>? cities;

        private Country selectedCountry = new Country();
        private State selectedState = new State();
        private City selectedCity = new City();

        [Parameter] public bool IsEdit { get; set; } = false;
        [Parameter, SupplyParameterFromQuery] public string CityId { get; set; } = string.Empty;

        [EditorRequired, Parameter] public ResidentialUnit ResidentialUnit { get; set; } = null!;
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        public bool FormPostedSuccessfully { get; set; }

        protected override async Task OnInitializedAsync()
        {
            editContext = new(ResidentialUnit);
            if (!IsEdit)
            {
                await LoadCountriesAsync();
            }
            else
            {
                if (ResidentialUnit != null)
                {
                    selectedCountry = ResidentialUnit.City!.State!.Country!;
                    selectedState = ResidentialUnit.City!.State!;
                    selectedCity = ResidentialUnit.City!;
                }
                await LoadCountriesAsync();
                await LoadStatesAsync(ResidentialUnit!.City!.State!.Country!.Id);
                await LoadCitiesAsync(ResidentialUnit!.City!.State!.Id);
            }
        }

        private async Task<IEnumerable<Country>> SearchCountries(string searchText)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return countries!;
            }

            return countries!
                .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<IEnumerable<State>> SearchStates(string searchText)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return states!;
            }

            return states!
                .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<IEnumerable<City>> SearchCity(string searchText)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return cities!;
            }

            return cities!
                .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task LoadCountriesAsync()
        {
            loading = true;
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            countries = responseHttp.Response;
            loading = false;
        }

        private async Task LoadStatesAsync(int countryId)
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

        private async Task LoadCitiesAsync(int stateId)
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

        private async Task CountryChangedAsync(Country country)
        {
            selectedCountry = country;
            selectedState = new State();
            selectedCity = new City();
            states = null;
            cities = null;
            await LoadStatesAsync(country.Id);
        }

        private async Task StateChangedAsync(State state)
        {
            selectedState = state;
            selectedCity = new City();
            cities = null;
            ResidentialUnit.CityId = 0;
            await LoadCitiesAsync(state.Id);
        }

        private void CityChangedAsync(City city)
        {
            selectedCity = city;
            ResidentialUnit.CityId = city!.Id;
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formWasEdited = editContext.IsModified();

            if (!formWasEdited || FormPostedSuccessfully)
            {
                return;
            }

            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmaci�n",
                Text = "�Deseas abandonar la p�gina y perder los cambios?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });

            var confirm = !string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            context.PreventNavigation();
        }
    }
}