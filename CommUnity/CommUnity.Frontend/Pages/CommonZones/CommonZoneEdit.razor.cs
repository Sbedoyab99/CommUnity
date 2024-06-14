using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.CommonZones
{
    public partial class CommonZoneEdit
    {
        private CommonZone? commonZone;
        private CommonZoneForm? commonZoneForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [EditorRequired, Parameter] public int CommonZoneId { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<CommonZone>($"api/commonzones/{CommonZoneId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
            }
            else
            {
                commonZone = responseHttp.Response!;
            }
        }

        private async Task EditAsync()
        {
            if (commonZone == null)
            {
                return;
            }
            var responseHttp = await Repository.PutAsync("api/commonzones", ToCommonZoneDTO(commonZone));
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
                Title = "Zona Com�n editada",
                Icon = SweetAlertIcon.Success,
            });
        }

        private CommonZoneDTO ToCommonZoneDTO(CommonZone commonZone)
        {
            return new CommonZoneDTO
            {
                Id = commonZone.Id,
                ResidentialUnitId = commonZone.ResidentialUnitId,
                Name = commonZone.Name,
                Capacity = commonZone.Capacity
            };
        }

        private void Return()
        {
            commonZoneForm!.FormPostedSuccesfully = true;
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}