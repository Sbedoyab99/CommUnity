using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Pages.Cities;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics.Metrics;
using System.Net;

namespace CommUnity.FrontEnd.Pages.States
{
    public partial class StateDetails
    {
        private State? state;
        private List<City>? cities;

        private MudTable<City> table = new();
        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";
        private int totalRecords = 0;
        private bool loading;

        [Parameter] public int StateId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;       

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await LoadTotalRecords();
        }

        private async Task<bool> LoadStateAsync()
        {
            var responseHttp = await Repository.GetAsync<State>($"/api/states/{StateId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/countries");
                    return false;
                }

                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            state = responseHttp.Response;
            return true;
        }

        private async Task<bool> LoadTotalRecords()
        {
            loading = true;
            if (state is null)
            {
                var ok = await LoadStateAsync();
                if (!ok)
                {
                    NoState();
                    return false;
                }
            }
            string baseUrl = "api/cities";
            string url;

            url = $"{baseUrl}/recordsnumber?id={StateId}&page=1&recordsnumber={int.MaxValue}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
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
                return false;
            }
            totalRecords = responseHttp.Response;
            loading = false;
            return true;
        }

        private async Task<TableData<City>> LoadListAsync(TableState state)
        {
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = $"api/cities";
            string url;

            url = $"{baseUrl}?id={StateId}&page={page}&recordsnumber={pageSize}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<City>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<City> { Items = new List<City>(), TotalItems = 0 };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<City> { Items = new List<City>(), TotalItems = 0 };
            }
            return new TableData<City>
            {
                Items = responseHttp.Response,
                TotalItems = totalRecords
            };
        }

        private async Task SetFilterValue(string value)
        {
            Filter = value;
            await LoadAsync();
            await table.ReloadServerData();
        }

        private void ReturnAction()
        {
            NavigationManager.NavigateTo($"/countries/details/{state?.CountryId}");
        }

        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IDialogReference modal;

            if (isEdit)
            {
                var parameters = new DialogParameters<CityEdit> { { x => x.CityId, id } };
                modal = DialogService.Show<CityEdit>("Editar Ciudad", parameters);
            }
            else
            {
                var parameters = new DialogParameters<CityCreate> { { x => x.StateId, StateId } };
                modal = DialogService.Show<CityCreate>("Crear Ciudad", parameters);
            }

            var result = await modal.Result;
            if (!result.Canceled)
            {
                await LoadAsync();
                await table.ReloadServerData();
            }         
        }

        private void ResidentialUnitsAction(City city)
        {
            NavigationManager.NavigateTo($"/cities/details/{city.Id}");
        }

        private void NoState()
        {
            NavigationManager.NavigateTo("/countries");
        }

        private async Task DeleteAsync(City city)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estas seguro?",
                Text = $"¿Estas seguro de que quieres eliminar la ciudad {city.Name}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<City>($"api/cities/{city.Id}");
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
            await table.ReloadServerData();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync("Ciudad eliminada", string.Empty, SweetAlertIcon.Success);
        }
    }
}