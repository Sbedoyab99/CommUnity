using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.MyApartment
{
    public partial class Listvisitors
    {
        private bool loading = true;
        private List<VisitorEntry> visitors;
        private MudTable<VisitorEntry> tableV = new();

        [Parameter] public int ApartmentId { get; set; }
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [CascadingParameter] IModalService Modal { get; set; } = default!;
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            loading = false;
        }

        private async Task<TableData<VisitorEntry>> LoadAllVisitorEntry(TableState state) {
            string baseUrl = $"/api/VisitorEntry/full";
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
            var responseHttp = await Repository.PutAsync(url, visitorEntry);
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

        private async Task Return()
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }


    }
}