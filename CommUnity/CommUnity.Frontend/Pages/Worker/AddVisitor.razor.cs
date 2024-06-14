using Blazored.Modal.Services;
using Blazored.Modal;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using CommUnity.Shared.Entities;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Worker
{
    public partial class AddVisitor
    {
        private List<Apartment> apartments = new();
        private VisitorEntryDTO visitorEntryDTO = new();
        private Apartment selectedApartment = new();
        public bool loading = true;
        public bool FormPostedSuccesfully { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] public IRepository Repository { get; set; } = null!;

        [Parameter] public int ResidentialUnitId { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            visitorEntryDTO.Date = DateTime.Now;
            await LoadApartmentsAsync();
        }

        private async Task LoadApartmentsAsync()
        {
            string baseUrl = $"api/apartments";
            string url;

            url = $"{baseUrl}?id={ResidentialUnitId}";

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
                return;
            }
            if (responseHttp.Response == null)
            {
                return;
            }
            apartments = responseHttp.Response;
            loading = false;
            return;
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
            var responseHttp = await Repository.PostAsync("/api/visitorentry/add", visitorEntryDTO);
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
            MudDialog.Close(DialogResult.Ok(true));
        }

        private void Return()
        {
            MudDialog.Close(DialogResult.Cancel());
        }

        private void ApartmentSelected(Apartment apartment)
        {
            selectedApartment = apartment;
            visitorEntryDTO.ApartmentId = apartment.Id;
        }

        private async Task<IEnumerable<Apartment>> SearchApartment(string searchText)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return apartments!;
            }

            return apartments!
                .Where(c => c.Number.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }
}