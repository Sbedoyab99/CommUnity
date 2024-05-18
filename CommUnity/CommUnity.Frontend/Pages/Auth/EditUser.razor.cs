using Blazored.Modal.Services;
using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Services;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.Metrics;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Auth
{
    [Authorize]
    public partial class EditUser
    {
        private User? user;
        private List<Country>? countries;
        private List<State>? states;
        private List<City>? cities;
        private List<ResidentialUnit>? residentialUnits;
        private List<Apartment>? apartments;
        private bool loading = true;
        private string? imageUrl;

        private Country selectedCountry = new ();
        private State selectedState = new ();
        private City selectedCity = new ();
        private ResidentialUnit selectedResidentialUnit = new ();
        private Apartment selectedApartment = new ();
        private UserType selectedUserType = UserType.Resident;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private List<UserType> userTypes = new List<UserType>
        {
            UserType.Resident,
            UserType.Worker,
            UserType.AdminResidentialUnit
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadUserAsyc();

            selectedCountry = user!.City!.State!.Country!;
            selectedState = user.City.State;
            selectedCity = user.City;
            selectedResidentialUnit = user!.ResidentialUnit!;
            selectedApartment = user!.Apartment!;
            selectedUserType = user.UserType!;

            await LoadCountriesAsync();
            await LoadStatesAsyn(user!.City!.State!.Country!.Id);
            await LoadCitiesAsyn(user!.City!.State!.Id);
            await LoadResidentialUnitsAsync(user!.CityId);
            await LoadApartmentsAsync(user!.ResidentialUnitId);

            if (!string.IsNullOrEmpty(user!.Photo))
            {
                imageUrl = user.Photo;
                user.Photo = null;
            }

        }

        private async Task LoadUserAsyc()
        {
            var responseHttp = await Repository.GetAsync<User>($"/api/accounts");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                    return;
                }
                var messageError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }
            user = responseHttp.Response;
            loading = false;
        }

        private void ImageSelected(string imagenBase64)
        {
            user!.Photo = imagenBase64;
            imageUrl = null;
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
        }

        private async Task LoadResidentialUnitsAsync(int cityId)
        {
            var responseHttp = await Repository.GetAsync<List<ResidentialUnit>>($"/api/residentialunit?id={cityId}&page=1&recordsnumber={int.MaxValue}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            residentialUnits = responseHttp.Response;
        }

        private async Task LoadApartmentsAsync(int? residentialUnitId)
        {
            var responseHttp = await Repository.GetAsync<List<Apartment>>($"/api/apartments?id={residentialUnitId}&page=1&recordsnumber={int.MaxValue}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            apartments = responseHttp.Response;
        }

        private async Task CountryChangedAsync(Country country)
        {
            selectedCountry = country;
            selectedState = new State();
            selectedCity = new City();
            selectedResidentialUnit = new ResidentialUnit();
            selectedApartment = new Apartment();
            states = null;
            cities = null;
            residentialUnits = null;
            apartments = null;
            await LoadStatesAsyn(country.Id);
        }

        private async Task StateChangedAsync(State state)
        {
            selectedState = state;
            selectedCity = new City();
            selectedResidentialUnit = new ResidentialUnit();
            selectedApartment = new Apartment();
            cities = null;
            residentialUnits = null;
            apartments = null;
            await LoadCitiesAsyn(state.Id);
        }

        private async Task CityChangedAsync(City city)
        {
            selectedCity = city;
            selectedResidentialUnit = new ResidentialUnit();
            selectedApartment = new Apartment();
            residentialUnits = null;
            apartments = null;
            user!.CityId = city.Id;
            await LoadResidentialUnitsAsync(city.Id);
        }

        private async Task ResidentialUnitChangedAsync(ResidentialUnit residentialUnit)
        {
            selectedResidentialUnit = residentialUnit;
            selectedApartment = new Apartment();
            user!.ResidentialUnitId = residentialUnit.Id;
            user.Address = residentialUnit.Address;
            apartments = null;
            await LoadApartmentsAsync(residentialUnit.Id);
        }

        private void ApartmentChangedAsync(Apartment apartment)
        {
            selectedApartment = apartment;
            user!.ApartmentId = apartment.Id;
        }

        private void UserTypeChanged(UserType userType)
        {
            selectedUserType = userType;
            user!.UserType = selectedUserType;
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

        private async Task<IEnumerable<ResidentialUnit>> SearchResidentialUnit(string searchText)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return residentialUnits!;
            }

            return residentialUnits!
                .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<IEnumerable<Apartment>> SearchApartment(string searchText)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return apartments!;
            }

            return apartments!
                .Where(c => c.Number.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<IEnumerable<UserType>> SearchRole(string searchText)
        {
            await Task.Delay(5);
            return userTypes!;
        }

        private async Task SaveUserAsync()
        {
            var responseHttp = await Repository.PutAsync<User, TokenDTO>("/api/accounts", user!);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await LoginService.LoginAsync(responseHttp.Response!.Token);
            await SweetAlertService.FireAsync("Confirmación", "Usuario Modificado con éxito.", SweetAlertIcon.Info);

            NavigationManager.NavigateTo("/");
        }
        private void ReturnAction()
        {

            NavigationManager.NavigateTo("/");
        }
    }
}
