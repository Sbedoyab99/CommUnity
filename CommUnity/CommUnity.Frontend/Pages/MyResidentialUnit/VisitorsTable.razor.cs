using CommUnity.FrontEnd.Pages.Worker;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.MyResidentialUnit
{
    public partial class VisitorsTable
    {
        private List<VisitorEntry>? visitors;
        private MudTable<VisitorEntry> table = new();

        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";

        private int totalRecords = 0;
        private bool loading = true;
        private VisitorStatus Status = VisitorStatus.Scheduled;

        [Parameter] public int? ResidentialUnitId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await GetRecordsNumber();
        }

        private async Task GetRecordsNumber()
        {
            //string baseUrl = $"api/visitorentry/recordsnumber";
            //string url = $"{baseUrl}?id={ResidentialUnitId}&status={Status}";

            string baseUrl = $"api/visitorentry";
            string url;

            url = $"{baseUrl}/RecordsNumberResidentialUnit?Id={ResidentialUnitId}&status={Status}&page=1&recordsnumber={int.MaxValue}";

            var responseHttp = await Repository.GetAsync<int>(url);
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
            totalRecords = responseHttp.Response;
            loading = false;
            return;
        }

        private async Task<TableData<VisitorEntry>> LoadVisitorsAsync(TableState state)
        {

            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = $"api/visitorentry";
            string url;

            url = $"{baseUrl}/VisitorEntryResidentialUnitStatus?Id={ResidentialUnitId}&status={Status}&page={page}&recordsnumber={pageSize}";

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
                return new TableData<VisitorEntry> { Items = new List<VisitorEntry>(), TotalItems = 0 };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<VisitorEntry> { Items = new List<VisitorEntry>(), TotalItems = 0 };
            }
            return new TableData<VisitorEntry>
            {
                Items = responseHttp.Response,
                TotalItems = totalRecords
            };
        }

        private async Task ChangedValue(VisitorStatus status)
        {
            Status = status;
            await LoadAsync();
            await table.ReloadServerData();
        }

    }
}