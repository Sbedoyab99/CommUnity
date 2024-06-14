using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.MyApartment
{
    public partial class ScheduleVisitor
    {
        private VisitorEntryDTO visitorEntryDTO = new();
        public bool loading { get; set; }
        public bool FormPostedSuccesfully { get; set; }

        [Parameter] public int ApartmentId { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] public IRepository Repository { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        protected override void OnInitialized()
        {
            visitorEntryDTO.Date = DateTime.Now;
            visitorEntryDTO.ApartmentId = ApartmentId;
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
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            MudDialog.Close();
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "La visita ha sido progrmada.",
                Icon = SweetAlertIcon.Success,
            });           
            return;
        }

        private void Return()
        {
            MudDialog.Close();
        }
    }
}