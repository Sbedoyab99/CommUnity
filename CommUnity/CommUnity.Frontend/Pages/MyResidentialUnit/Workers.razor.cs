using CommUnity.FrontEnd.Pages.ResidentialUnits;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace CommUnity.FrontEnd.Pages.MyResidentialUnit
{
    public partial class Workers
    {
        public List<User>? workers { get; set; }
        public User? _user = null;

        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";

        private MudTable<User> table = new();
        private int totalRecords = 0;
        private bool loading;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

        [Inject] private IDialogService DialogService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await GetUserAsync();
            if (_user == null)
            {
                NavigationManager.NavigateTo("/");
                return;
            }
            await LoadTotalRecords();
        }

        private async Task GetUserAsync()
        {
            var responseHttp = await Repository.GetAsync<User>($"/api/accounts");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }
                return;
            }
            _user = responseHttp.Response!;
            return;
        }

        private async Task<bool> LoadTotalRecords()
        {
            loading = true;
            string baseUrl = "api/workers";
            string url;

            url = $"{baseUrl}/recordsnumber?id={_user?.ResidentialUnitId}&page=1&recordsnumber={int.MaxValue}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
            }
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
                return false;
            }
            totalRecords = responseHttp.Response;
            loading = false;
            return true;
        }

        private async Task<TableData<User>> LoadListAsync(TableState state)
        {
            await GetUserAsync();
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = "api/workers/workers";
            string url;

            url = $"{baseUrl}?id={_user?.ResidentialUnitId}&page=1&page={page}&recordsnumber={pageSize}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<User>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<User> { Items = new List<User>(), TotalItems = 0 };
            }

            if (responseHttp.Response == null)
            {
                return new TableData<User> { Items = new List<User>(), TotalItems = 0 };
            }
            return new TableData<User>
            {
                Items = responseHttp.Response,
                TotalItems = totalRecords
            };
        }

        private async Task SetFilterValue(string value)
        {
            Filter = value;
            await LoadAsync();
            await table.ReloadServerData();
        }

        private async Task WorkerAction(ResidentialUnit residentialUnit)
        {
            IDialogReference modal;

            var parameters = new DialogParameters<NewWorkerForm> { { x => x.ResidentialUnit, residentialUnit } };
            modal = DialogService.Show<NewWorkerForm>("Crear Trabajador", parameters);

            var result = await modal.Result;
            if (!result.Canceled)
            {
                await LoadAsync();
                await table.ReloadServerData();
            }
        }
    }
}