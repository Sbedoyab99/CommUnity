using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Pqrss
{
    public partial class PqrsIndexAdmin
    {

        [Parameter] public int ResidentialUnitId { get; set; }

        private List<Pqrs>? pqrss;
        private MudTable<Pqrs> table = new();
        private List<Apartment>? apartments;

        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";

        private int totalRecords = 0;
        private bool loading = true;
        private PqrsType Type;
        private PqrsState Status;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        private Apartment selectedApartment = new Apartment();

        private List<PqrsType> types = new List<PqrsType>
        {
            PqrsType.Request,
            PqrsType.Complaint,
            PqrsType.Claim,
            PqrsType.Suggestion
        };

        private List<PqrsState> states = new List<PqrsState>
        {
            PqrsState.Settled,
            PqrsState.InReview,
            PqrsState.InProgress,
            PqrsState.Resolved,
            PqrsState.Closed
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await LoadApartmentsAsync(ResidentialUnitId);
            await GetRecordsNumber();
        }
        private async Task LoadApartmentsAsync(int residentialUnitId)
        {
            var responseHttp = await Repository.GetAsync<List<Apartment>>($"/api/apartments?id={residentialUnitId}&page=1&recordsnumber={int.MaxValue}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            apartments = responseHttp.Response;
        }

        private async Task GetRecordsNumber()
        {
            loading = true;
            string baseUrl = $"api/Pqrss";
            string url;

            url = $"{baseUrl}/recordsnumber?ResidentialUnitId={ResidentialUnitId}&type={Type}&status={Status}&page=1&recordsnumber={int.MaxValue}&ApartmentId={(selectedApartment != null ? selectedApartment.Id : 0)}";

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

        private async Task<TableData<Pqrs>> LoadPqrssAdminAsync(TableState state)
        {
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = $"api/Pqrss";
            string url;

            url = $"{baseUrl}/pqrss?ResidentialUnitId={ResidentialUnitId}&type={Type}&status={Status}&page={page}&recordsnumber={pageSize}&ApartmentId={(selectedApartment != null ? selectedApartment.Id : 0)}";

            var responseHttp = await Repository.GetAsync<List<Pqrs>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<Pqrs> { Items = new List<Pqrs>(), TotalItems = 0 };
            }
            if (responseHttp.Response == null)
            {
                return new TableData<Pqrs> { Items = new List<Pqrs>(), TotalItems = 0 };
            }
            return new TableData<Pqrs>
            {
                Items = responseHttp.Response,
                TotalItems = totalRecords
            };
        }

        private async Task<IEnumerable<PqrsType>> SearchType(string searchText)
        {
            await Task.Delay(5);
            return types!;
        }

        private async Task<IEnumerable<PqrsState>> SearchState(string searchText)
        {
            await Task.Delay(5);
            return states!;
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

        private async Task ChangedValueType(PqrsType type)
        {
            Type = type;
            await LoadAsync();
            await table.ReloadServerData();
        }

        private async Task ChangedValueStatus(PqrsState status)
        {
            Status = status;
            await LoadAsync();
            await table.ReloadServerData();
        }
        private async Task ApartmentChangedAsync(Apartment apartment)
        {
            selectedApartment = apartment;
            await LoadAsync();
            await table.ReloadServerData();
        }

        private async Task EditStatusPqrs(PqrsState pqrsState, int PqrsId = 0)
        {
            IDialogReference modal;

            int currentStateIndex = states.IndexOf(pqrsState);
            var filteredStates = states.Where(state => states.IndexOf(state) > currentStateIndex).ToList();
            var parameters = new DialogParameters
            {
                { "Id", PqrsId },
                { "AvailableStates", filteredStates },
                { "CurrentState", pqrsState }
            };

            var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true, CloseOnEscapeKey = true, DisableBackdropClick = true };
            modal = DialogService.Show<UpdateStatusPqrs>($"Actualizar Estado PQRS - {PqrsId}", parameters, options);

            var result = await modal.Result;
            if (!result.Canceled)
            {
                await table.ReloadServerData();
            }

        }

        private async Task DetailPqrs(int PqrsId = 0, bool isEdit = false)
        {
            IDialogReference modal;

            if (isEdit)
            {
                var parameters = new DialogParameters<Pqrs> { { x => x.Id, PqrsId } };
                var options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true, CloseOnEscapeKey = true, DisableBackdropClick = true };
                modal = DialogService.Show<ViewPqrs>($"Detalle PQRS - {PqrsId}", parameters, options);

                var result = await modal.Result;
                if (!result.Canceled)
                {
                    await table.ReloadServerData();
                }
            }
        }

    }
}