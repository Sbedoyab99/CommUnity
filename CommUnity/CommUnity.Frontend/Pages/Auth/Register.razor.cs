using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Services;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.Auth
{
    public partial class Register
    {
        private UserDTO userDTO = new();
        private List<Country>? countries;
        private List<State>? states;
        private List<City>? cities;
        private List<ResidentialUnit>? residentialUnits;
        private List<Apartment>? apartments;
        private bool loading;
        private string? imageUrl;

        private Country selectedCountry = new Country();
        private State selectedState = new State();
        private City selectedCity = new City();
        private ResidentialUnit selectedResidentialUnit = new ResidentialUnit();
        private Apartment selectedApartment = new Apartment();
        private UserType selectedUserType = UserType.Resident;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private ILoginService LogInService { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<UserType> userTypes = new List<UserType>
        {
            UserType.Resident,
            UserType.Worker,
            UserType.AdminResidentialUnit
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync();
        }

        private void ImageSelected(string imageBase64)
        {
            userDTO.Photo = imageBase64;
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

        private async Task LoadApartmentsAsync(int residentialUnitId)
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
            userDTO.CityId = city.Id;
            await LoadResidentialUnitsAsync(city.Id);
        }

        private async Task ResidentialUnitChangedAsync(ResidentialUnit residentialUnit)
        {
            selectedResidentialUnit = residentialUnit;
            selectedApartment = new Apartment();
            userDTO.ResidentialUnitId = residentialUnit.Id;
            userDTO.Address = residentialUnit.Address;
            apartments = null;
            await LoadApartmentsAsync(residentialUnit.Id);
        }

        private void ApartmentChangedAsync(Apartment apartment)
        {
            selectedApartment = apartment;
            userDTO.ApartmentId = apartment.Id;
        }

        private void UserTypeChanged(UserType userType)
        {
            selectedUserType = userType;
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

        private void ReturnAction()
        {
            NavigationManager.NavigateTo("/");
        }

        private async Task CreateUserAsync()
        {
            userDTO.UserName = userDTO.Email;
            userDTO.UserType = selectedUserType;
            loading = true;
            var responseHttp = await Repository.PostAsync<UserDTO>("/api/accounts/CreateUser", userDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            await SweetAlertService.FireAsync("Confirmaci�n", "Su cuenta ha sido creada con exito. Se te ha enviado un correo electr�nico con las instrucciones para activar tu usuario.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/");
        }
    }
}
