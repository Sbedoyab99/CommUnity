using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Pages.Auth;
using CommUnity.FrontEnd.Pages.Pets;
using CommUnity.FrontEnd.Pages.Pqrss;
using CommUnity.FrontEnd.Pages.Vehicles;
using CommUnity.FrontEnd.Pages.Worker;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace CommUnity.FrontEnd.Pages.MyApartment
{
    public partial class MyApartment
    {
        private List<Pet>? pets;
        private MudTable<Pet> tableP = new();
        private List<Vehicle>? vehicles;
        private MudTable<Vehicle> tableV = new();
        private List<User>? users;
        private MudTable<User> tableU = new();
        private bool loading = true;
        private Apartment apartment = null!;

        [Parameter] public int ApartmentId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadApartmentAsync();
            await LoadUsersAsync();
            loading = false;
        }

        private async Task LoadApartmentAsync()
        {
            var responseHttp = await Repository.GetAsync<Apartment>($"/api/apartments/{ApartmentId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                    return;
                }

                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            apartment = responseHttp.Response!;
            return;
        }

        private async Task<TableData<Pet>> LoadPetAsync(TableState state)
        {

            string baseUrl = $"api/pets";
            string url;

            url = $"{baseUrl}?id={ApartmentId}";

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
                return new TableData<Pet> { Items = new List<Pet>() };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<Pet> { Items = new List<Pet>() };
            }
            return new TableData<Pet>
            {
                Items = responseHttp.Response
            };

        }
        private async Task<TableData<Vehicle>> LoadVehiclesAsync(TableState state)
        {

            string baseUrl = $"api/vehicles";
            string url;

            url = $"{baseUrl}?id={ApartmentId}";

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
                return new TableData<Vehicle> { Items = new List<Vehicle>() };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<Vehicle> { Items = new List<Vehicle>() };
            }
            return new TableData<Vehicle>
            {
                Items = responseHttp.Response
            };
        }

        private async Task LoadUsersAsync()
        {
            string baseUrl = $"api/resident/resident";
            string url = $"{baseUrl}?apartmentId={ApartmentId}";

            var responseHttp = await Repository.GetAsync<List<User>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                users = new List<User>();
            }
            else
            {
                users = responseHttp.Response ?? new List<User>();
            }
        }

        private void VisitorManagementModal()
        {
            var parameters = new DialogParameters<Listvisitors> { { x => x.ApartmentId, ApartmentId } };
            DialogService.Show<VisitorManagement>("Gestionar Visitas", parameters);
        }

        private void EmailManagement()
        {
            var parameters = new DialogParameters<Pqrs> { { x => x.ApartmentId, ApartmentId } };
            var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true, CloseOnEscapeKey = true, DisableBackdropClick = true };
            DialogService.Show<EmailManagement>("Gestión Correspondencia", parameters, options);
        }

        private void Soon()
        {
            NavigationManager.NavigateTo("/soon");
        }

        private async Task PetModal(int PetId = 0, bool isEdit = false)
        {
            IDialogReference modal;

            if (isEdit)
            {
                var parameters = new DialogParameters<PetEdit> { { x => x.PetId, PetId } };
                modal = DialogService.Show<PetEdit>("Editar Mascota", parameters);
            }
            else
            {
                var parameters = new DialogParameters<PetCreate> { { x => x.ApartmentId, ApartmentId } };
                modal = DialogService.Show<PetCreate>("Crear Mascota", parameters);
            }

            var result = await modal.Result;
            if (!result.Canceled)
            {
                await tableP.ReloadServerData();
            }
        }

        private async Task DeletePetAsync(Pet pet)
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
            await tableP.ReloadServerData();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync("Registro Eliminado", string.Empty, SweetAlertIcon.Success);
        }

        //------------------------------------------------
        private async Task VehicleModal(int VehicleId = 0, bool isEdit = false)
        {
            IDialogReference modal;

            if (isEdit)
            {
                var parameters = new DialogParameters<VehicleEdit> { { x => x.VehicleId, VehicleId } };
                modal = DialogService.Show<VehicleEdit>("Editar Vehiculo", parameters);
            }
            else
            {
                var parameters = new DialogParameters<VehicleCreate> { { x => x.ApartmentId, ApartmentId } };
                modal = DialogService.Show<VehicleCreate>("Crear Vehiculo", parameters);
            }

            var result = await modal.Result;
            if (!result.Canceled)
            {
                await tableV.ReloadServerData();
            }
        }

        private async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estás seguro?",
                Text = $"¿Estás seguro de que quieres eliminar el vehiculo {vehicle.Plate}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<Vehicle>($"api/vehicles/{vehicle.Id}");
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
            await tableV.ReloadServerData();
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