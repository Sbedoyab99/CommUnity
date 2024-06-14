﻿using CommUnity.FrontEnd.Pages.ResidentialUnits;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Cities
{
    public partial class ResidentialUnitCreateWithCity
    {
        ResidentialUnit residentialUnit = new();
        ResidentialUnitFormWithCity? residentialUnitFormWithCity;

        [Parameter] public int CityId { get; set; }

        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private async Task CreateResidentialUnitAsync()
        {
            residentialUnit.City = null;
            var responseHttp = await Repository.PostAsync("/api/residentialUnit", residentialUnit);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
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
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con exito.");
        }

        private void Return()
        {
            residentialUnitFormWithCity!.FormPostedSuccessfully = true;
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}