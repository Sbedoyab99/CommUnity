using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Vehicles
{

    public partial class VehicleIndex
    {

        private Apartment? apartment;
        private List<Vehicle>? vehicle;

        private int currentPage = 1;
        private int totalPages;
        private string currentRecordsNumber = "10";

        [Parameter] public int ApartmentId { get; set; }
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string RecordsNumber { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task<bool> LoadApartmentAsync()
        {
            var responseHttp = await Repository.GetAsync<Apartment>($"/api/apartments/{ApartmentId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                    return false;
                }

                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            apartment = responseHttp.Response;
            return true;
        }

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }
            var ok = await LoadApartmentAsync();
            if (ok)
            {
                ok = await LoadListAsync(page);
                if (ok)
                {
                    if (RecordsNumber != "todos")
                    {
                        await LoadPagesAsync();
                    }
                }
            }
        }

        private async Task<bool> LoadListAsync(int page)
        {
            string baseUrl = $"api/vehicles";
            string url;
            if (currentRecordsNumber == "todos")
            {
                url = $"{baseUrl}/all?id={ApartmentId}";
            }
            else
            {
                url = $"{baseUrl}?id={ApartmentId}&page={page}&recordsnumber={currentRecordsNumber}";
                if (!string.IsNullOrWhiteSpace(Filter))
                {
                    url += $"&filter={Filter}";
                }

            }
            var responseHttp = await Repository.GetAsync<List<Vehicle>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
            }
            vehicle = responseHttp.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            string baseUrl = $"api/vehicles";
            string url;
            if (currentRecordsNumber == "todos")
            {
                return;
            }
            else
            {
                url = $"{baseUrl}/totalpages?id={ApartmentId}&recordsnumber={currentRecordsNumber}";
                if (!string.IsNullOrWhiteSpace(Filter))
                {
                    url += $"&filter={Filter}";
                }
            }

            var responseHttp = await Repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
            }
            totalPages = responseHttp.Response;
        }

        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        private async Task SetFilterValue(string value)
        {
            Filter = value;
            await ApplyFilterAsync();
        }

        private async Task SetRecordsNumber(string value)
        {
            int page = 1;
            RecordsNumber = value;
            currentRecordsNumber = value;
            await LoadAsync(page);
        }

        private async Task DeleteAsync(Vehicle  vehicle)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estás seguro?",
                Text = $"¿Estás seguro de que quieres eliminar el vehículo {vehicle.Plate}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<Vehicle>($"api/vehicles/{vehicle.Plate}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "Error",
                        Text = message,
                        Icon = SweetAlertIcon.Error
                    });
                }
                return;
            }
            await LoadAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync("Registro Eliminado", string.Empty, SweetAlertIcon.Success);
        }

    }


}