using CommUnity.FrontEnd.Pages.MyResidentialUnit;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.Worker
{
    public partial class MailTable
    {
        private List<MailArrival>? mails;
        private MudTable<MailArrival> tableM = new();

        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";

        private int totalRecords = 0;
        private bool loading = true;
        private MailStatus Status = MailStatus.Stored;

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
            string baseUrl = $"api/mail";
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

        private async Task<TableData<MailArrival>> LoadMailAsync(TableState state)
        {
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = $"api/mail";
            string url;

            url = $"{baseUrl}/MailResidentialUnitStatus?Id={ResidentialUnitId}&status={Status}&page={page}&recordsnumber={pageSize}";

            var responseHttp = await Repository.GetAsync<List<MailArrival>>(url);
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

        private async Task RegisterMailAsync()
        {
            IDialogReference modal;

            var parameters = new DialogParameters<RegisterMail> { { x => x.ResidentialUnitId, ResidentialUnitId } };
            modal = DialogService.Show<RegisterMail>("Registrar Correspondencia", parameters);
            Console.WriteLine("Abriendo modal");
            var result = await modal.Result;
            Console.WriteLine(result);
            if (!result.Canceled)
            {
                Console.WriteLine("Mail registered");
                await LoadAsync();
                await tableM.ReloadServerData();
            }
        }

        private async Task ChangedValue(MailStatus status)
        {
            Status = status;
            await LoadAsync();
            await tableM.ReloadServerData();
        }

        private MailArrivalDTO ToMailDTO(MailArrival mailArrival)
        {
            return new MailArrivalDTO
            {
                Sender = mailArrival.Sender!,
                Type = mailArrival.Type!,
                Id = mailArrival.Id,
                Status = mailArrival.Status
            };
        }
    }
}