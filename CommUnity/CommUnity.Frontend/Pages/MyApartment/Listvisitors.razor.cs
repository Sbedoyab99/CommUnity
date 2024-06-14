using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.MyApartment
{
    public partial class Listvisitors
    {
        private bool loading = true;
        private List<VisitorEntry> visitors = new();
        private MudTable<VisitorEntry> tableV = new();

        [Parameter] public int ApartmentId { get; set; }
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        protected override void OnInitialized()
        {
            loading = false;
        }

        private async Task<TableData<VisitorEntry>> LoadAllVisitorEntry(TableState state) {
            string baseUrl = $"/api/VisitorEntry/apartment/{ApartmentId}";
            string url;

            url = $"{baseUrl}";

            var responseHttp = await Repository.GetAsync<List<VisitorEntry>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<VisitorEntry> { Items = new List<VisitorEntry>() };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<VisitorEntry> { Items = new List<VisitorEntry>() };
            }
            return new TableData<VisitorEntry>
            {
                Items = responseHttp.Response
            };

        }

        private async Task CancelVisitorEntry(VisitorEntry visitorEntry)
        {
            visitorEntry.Status = CommUnity.Shared.Enums.VisitorStatus.Canceled;
            string url = $"/api/VisitorEntry/cancel";
            var responseHttp = await Repository.PutAsync(url, ToVisitorDTO(visitorEntry));
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
            }
            else
            {
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Éxito",
                    Text = "La visita ha sido cancelada",
                    Icon = SweetAlertIcon.Success
                });
                await tableV.ReloadServerData();
            }
        }

        private VisitorEntryDTO ToVisitorDTO(VisitorEntry visitorEntry)
        {
            return new VisitorEntryDTO
            {
                Date = visitorEntry.DateTime,
                Name = visitorEntry.Name!,
                Plate = visitorEntry.Plate!,
                Id = visitorEntry.Id,
                Status = VisitorStatus.Canceled
            };
        }

        private void Return()
        {
            MudDialog.Close();
        }
    }
}