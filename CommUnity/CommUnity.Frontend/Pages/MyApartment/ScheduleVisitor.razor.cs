using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Repositories;
using CommUnity.FrontEnd.Shared;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CommUnity.FrontEnd.Pages.MyApartment
{
    public partial class ScheduleVisitor
    {
        private VisitorEntryDTO visitorEntryDTO = new();
        public bool loading { get; set; }
        public bool FormPostedSuccesfully { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] public IRepository Repository { get; set; } = null!;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        protected override void OnInitialized()
        {
            visitorEntryDTO.Date = DateTime.Now;
        }

        private async Task OnDateChange(DateTime? date)
        {
            await Task.Delay(1);
            if (date == null)
            {
                return;
            }
            visitorEntryDTO.Date = (DateTime)date;
        }

        private async Task Submit()
        {
            loading = true;
            var responseHttp = await Repository.PostAsync("/api/visitorentry/schedule", visitorEntryDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            await SweetAlertService.FireAsync("Confirmación", "La visita ha sido progrmada.", SweetAlertIcon.Success);
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            }); 
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }

        private async Task Return()
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }
    }
}