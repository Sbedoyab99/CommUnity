using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Vehicles
{
    public partial class VehicleEdit
    {
        private Vehicle? vehicle;
        private VehicleForm? vehicleForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [EditorRequired, Parameter] public int VehicleId { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Vehicle>($"api/vehicles/{VehicleId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                Return();
            }
            else
            {
                vehicle = responseHttp.Response!;
            }
        }

        private async Task EditAsync()
        {
            if (vehicle == null)
            {
                return;
            }
            var responseHttp = await Repository.PutAsync("api/vehicles", ToVehicleDTO(vehicle));
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                return;
            }
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            MudDialog.Close(DialogResult.Ok(true));
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "Vehiculo editado",
                Icon = SweetAlertIcon.Success,
            });
        }

        private VehicleDTO ToVehicleDTO(Vehicle vehicle)
        {
            return new VehicleDTO
            {
                Id = vehicle.Id,
                ApartmentId = vehicle.ApartmentId,
                Plate = vehicle.Plate,
                Type = vehicle.Type,
                Description = vehicle.Description

            };
        }

        private void Return()
        {
            vehicleForm!.FormPostedSuccesfully = true;
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}