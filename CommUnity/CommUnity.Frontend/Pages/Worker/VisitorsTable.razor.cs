using CommUnity.FrontEnd.Pages.Pets;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Worker
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

        [Parameter] public int ResidentialUnitId { get; set; }

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
            string baseUrl = $"api/visitorentry/recordsnumber";
            string url = $"{baseUrl}?id={ResidentialUnitId}&status={Status}";
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

            string baseUrl = $"api/visitorentry/status";
            string url;

            url = $"{baseUrl}/{Status}";

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

        private async Task ConfirmVisitorEntry(VisitorEntry entry)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "¿Estás seguro?",
                Text = $"¿Estás seguro de que quieres actualizar el estado?",
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.PutAsync($"api/visitorentry/confirm", ToVisitorDTO(entry));
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "Error",
                        Text = message,
                        Icon = SweetAlertIcon.Error
                    });
                }
                return;
            }
            await table.ReloadServerData();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync("Registro Actualizado", string.Empty, SweetAlertIcon.Success);
        }

        private async Task AddVisitorAsync()
        {
            IDialogReference modal;

            var parameters = new DialogParameters<AddVisitor> { { x => x.ResidentialUnitId, ResidentialUnitId } };
            modal = DialogService.Show<AddVisitor>("Agregar Visitante", parameters);

            var result = await modal.Result;
            if (!result.Canceled)
            {
                await table.ReloadServerData();
            }
        }

        private async Task ChangedValue(VisitorStatus status)
        {
            Status = status;
            await LoadAsync();
            await table.ReloadServerData();
        }

        private VisitorEntryDTO ToVisitorDTO(VisitorEntry visitorEntry)
        {
            return new VisitorEntryDTO
            {
                Date = visitorEntry.DateTime,
                Name = visitorEntry.Name!,
                Plate = visitorEntry.Plate!,
                Id = visitorEntry.Id,
                Status = VisitorStatus.Approved
            };
        }
    }
}