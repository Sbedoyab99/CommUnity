using CommUnity.FrontEnd.Pages.Pqrss;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.MyApartment
{
    public partial class EmailManagement
    {
        private List<MailArrival>? mails;
        private MudTable<MailArrival> tableM = new();

        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";

        private int totalRecords = 0;
        private bool loading = true;
        private MailStatus Status = MailStatus.Stored;

        [Parameter] public int ApartmentId { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private void Return()
        {
            MudDialog.Close(DialogResult.Cancel());
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetRecordsNumber();
        }

        private async Task GetRecordsNumber()
        {
            loading = true;
            string baseUrl = $"api/mail";
            string url;

            url = $"{baseUrl}/RecordsNumberAparment?Id={ApartmentId}&status={Status}&page=1&recordsnumber={int.MaxValue}";

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

        private async Task<TableData<MailArrival>> LoadMailAsync(TableState state)
        {

            loading = true;
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = $"api/mail";
            string url;

            url = $"{baseUrl}/MailApartmentStatus?Id={ApartmentId}&status={Status}&page={page}&recordsnumber={pageSize}";

            var responseHttp = await Repository.GetAsync<List<MailArrival>>(url);

            loading = false;

            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<MailArrival> { Items = new List<MailArrival>(), TotalItems = 0 };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<MailArrival> { Items = new List<MailArrival>(), TotalItems = 0 };
            }
            return new TableData<MailArrival>
            {
                Items = responseHttp.Response,
                TotalItems = totalRecords
            };

        }

        private async Task ChangedValue(MailStatus status)
        {
            Status = status;
            await GetRecordsNumber();
            await tableM.ReloadServerData();
        }

        private async Task ActionEditStatus(int MailId = 0)
        {
            loading = true;

            var mailArrival = new MailArrivalDTO()
            {
                Id = MailId,
                Sender = "Default sender",
                Status = MailStatus.Delivered
            };

            var responseHttp = await Repository.PutAsync("/api/mail/updateStatus", mailArrival);

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
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "Correspondencia Registrada",
                Icon = SweetAlertIcon.Success,
            });
            await GetRecordsNumber();
            await tableM.ReloadServerData();

        }

    }
}