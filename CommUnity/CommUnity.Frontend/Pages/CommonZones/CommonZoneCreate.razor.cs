using CommUnity.FrontEnd.Pages.Apartments;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.CommonZones
{
    public partial class CommonZoneCreate
    {
        private CommonZoneForm? commonZoneForm;
        private CommonZone commonZone = new();

        [Parameter] public int ResidentialUnitId { get; set; }

        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            commonZone.ResidentialUnitId = ResidentialUnitId;
            var responseHttp = await Repository.PostAsync("api/commonzones", commonZone);
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
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "Zona Común creada",
                Icon = SweetAlertIcon.Success,
            });
            Return();
        }
        private void Return()
        {
            commonZoneForm!.FormPostedSuccesfully = true;
            NavigationManager.NavigateTo($"/commonzones/{ResidentialUnitId}");
        }
    }
}
