using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Pages.Apartments;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Pets
{

    public partial class PetIndex
    {
        private Apartment? apartment;
        private List<Pet>? pets;

        private MudTable<Pet> table = new();
        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private int totalRecords = 0;
        private bool loading;

        [Parameter] public int ApartmentId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

        [CascadingParameter] IModalService Modal { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await LoadTotalRecords();
        }

        private async Task<bool> LoadTotalRecords()
        {
            loading = true;
            if (apartment is null)
            {
                var ok = await LoadApartmentAsync();
                if (!ok)
                {
                    NoApartment();
                    return false;
                }
            }
            string baseUrl = "api/commonzones";
            string url;

            url = $"{baseUrl}/recordsnumber?id={ApartmentId}&page=1&recordsnumber={int.MaxValue}";
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

        private async Task<TableData<Pet>> LoadListAsync(TableState state)
        {
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = $"api/pets";
            string url;

            url = $"{baseUrl}?id={ApartmentId}&page={page}&recordsnumber={pageSize}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<Pet>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<Pet> { Items = new List<Pet>(), TotalItems = 0 };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<Pet> { Items = new List<Pet>(), TotalItems = 0 };
            }
            return new TableData<Pet>
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
            NavigationManager.NavigateTo($"/apartments/{apartment?.ResidentialUnitId}");
        }

        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IModalReference modalReference;

            if (isEdit)
            {
                modalReference = Modal.Show<PetEdit>(string.Empty, new ModalParameters().Add("PetId", id));
            }
            else
            {
                modalReference = Modal.Show<PetCreate>(string.Empty, new ModalParameters().Add("ApartmentId", ApartmentId));
            }

            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadAsync();
            }
            await table.ReloadServerData();
        }

        private void NoApartment()
        {
            NavigationManager.NavigateTo("/residentialunits");
        }

        private async Task DeleteAsync(Pet pet)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estás seguro?",
                Text = $"¿Estás seguro de que quieres eliminar la mascota {pet.Name}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<Pet>($"api/pets/{pet.Id}");
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