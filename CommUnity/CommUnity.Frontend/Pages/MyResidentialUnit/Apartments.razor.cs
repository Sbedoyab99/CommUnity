using CommUnity.FrontEnd.Pages.Apartments;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.MyResidentialUnit
{
    public partial class Apartments
    {
        private List<Apartment>? apartments;
        private User? _user = null;

        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";

        private MudTable<Apartment> table = new();
        private int totalRecords = 0;
        private bool loading;

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
            await GetUserAsync();
            if (_user == null)
            {
                NavigationManager.NavigateTo("/");
                return;
            }
            await LoadTotalRecords();
        }

        private async Task GetUserAsync()
        {
            var responseHttp = await Repository.GetAsync<User>($"/api/accounts");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }
                return;
            }
            _user = responseHttp.Response!;
            return;
        }

        private async Task<bool> LoadTotalRecords()
        {
            loading = true;
            string baseUrl = "api/apartments";
            string url;

            url = $"{baseUrl}/recordsnumber?id={_user?.ResidentialUnitId}&page=1&recordsnumber={int.MaxValue}";
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

        private async Task<TableData<Apartment>> LoadListAsync(TableState state)
        {
            await GetUserAsync();
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = $"api/apartments";
            string url;

            url = $"{baseUrl}?id={_user?.ResidentialUnitId}&page={page}&recordsnumber={pageSize}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<Apartment>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<Apartment> { Items = new List<Apartment>(), TotalItems = 0 };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<Apartment> { Items = new List<Apartment>(), TotalItems = 0 };
            }
            return new TableData<Apartment>
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

        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IDialogReference modal;
            DialogParameters parameters;

            if (isEdit)
            {
                parameters = new DialogParameters<ApartmentEdit> { { x => x.ApartmentId, id } };
                modal = DialogService.Show<ApartmentEdit>("Editar Apartamento", parameters);
            }
            else
            {
                parameters = new DialogParameters
                {
                    { "ResidentialUnitId", _user?.ResidentialUnitId }
                };
                modal = DialogService.Show<ApartmentCreate>("Crear Apartamento", parameters);
            }

            var result = await modal.Result;
            if (!result.Canceled)
            {
                await LoadAsync();
                await table.ReloadServerData();
            }
        }

        private void ShowApartmentAsync(Apartment apartment)
        {
            DialogParameters parameters;

            parameters = new DialogParameters
            {
                { "ApartmentId", apartment.Id }
            };

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };

            DialogService.Show<ApartmentModal>($"Apartamento - {apartment.Number}", parameters, options);
        }

        private async Task DeleteAsync(Apartment apartment)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "�Estas seguro?",
                Text = $"�Estas seguro de que quieres eliminar el apartamento {apartment.Number}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<ResidentialUnit>($"api/apartments/{apartment.Id}");
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
            await toast.FireAsync("Registro Eliminado", string.Empty, SweetAlertIcon.Success);
        }
    }
}