﻿using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Vehicles
{
    public partial class VehicleCreate
    {
        private VehicleForm? vehicleForm;
        private Vehicle vehicle = new();

        [Parameter] public int ApartmentId { get; set; }

        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private async Task CreateAsync()
        {
            vehicle.ApartmentId = ApartmentId;
            var responseHttp = await Repository.PostAsync("api/vehicles", vehicle);
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
                Title = "Vehículo creado",
                Icon = SweetAlertIcon.Success,
            });
        }

        private void Return()
        {
            vehicleForm!.FormPostedSuccesfully = true;
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}
