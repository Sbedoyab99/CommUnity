using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Pages.Cities;
using CommUnity.FrontEnd.Pages.Countries;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;
using System.Threading.Tasks;

namespace CommUnity.FrontEnd.Pages.ResidentialUnits
{
    public partial class ResidentialUnitsIndex
    {
        public List<ResidentialUnit>? ResidentialUnits { get; set; }

        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };
        private readonly string infoFormat = "{first_item}-{last_item} de {all_items}";

        private MudTable<ResidentialUnit> table = new();
        private int totalRecords = 0;
        private bool loading;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;

        [CascadingParameter] IModalService Modal { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            await LoadTotalRecords();
        }

        private async Task<bool> LoadTotalRecords()
        {
            loading = true;
            string baseUrl = "api/residentialUnit";
            string url;

            url = $"{baseUrl}/recordsnumber?page=1&recordsnumber={int.MaxValue}";
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

        private async Task<TableData<ResidentialUnit>> LoadListAsync(TableState state)
        {
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            string baseUrl = "api/residentialUnit";
            string url;

            url = $"{baseUrl}?page={page}&recordsnumber={pageSize}";
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<ResidentialUnit>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error
                });
                return new TableData<ResidentialUnit> { Items = new List<ResidentialUnit>(), TotalItems = 0 };
            }

            if (responseHttp.Response == null)
            {
                return new TableData<ResidentialUnit> { Items = new List<ResidentialUnit>(), TotalItems = 0 };
            }
            return new TableData<ResidentialUnit>
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

        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IModalReference modalReference;

            if (isEdit)
            {
                modalReference = Modal.Show<ResidentialUnitEdit>(string.Empty, new ModalParameters().Add("Id", id));
            }
            else
            {
                modalReference = Modal.Show<ResidentialUnitCreate>();
            }

            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadAsync();
            }
            await table.ReloadServerData();
        }

        private void ApartmentsAction(ResidentialUnit residentialUnit)
        {
            NavigationManager.NavigateTo($"/apartments/{residentialUnit.Id}");
        }

        private void CommonZonesAction(ResidentialUnit residentialUnit)
        {
            NavigationManager.NavigateTo($"/commonZones/{residentialUnit.Id}");
        }

        private void NewsAction(ResidentialUnit residentialUnit)
        {
            NavigationManager.NavigateTo($"/news/{residentialUnit.Id}");
        }

        private async Task DeleteAsync(ResidentialUnit residentialUnit)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "�Est�s seguro?",
                Text = $"�Est�s seguro de que quieres eliminar la unidad residencial {residentialUnit.Name}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<ResidentialUnit>($"api/residentialUnit/{residentialUnit.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/residentialUnit");
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
            await LoadAsync();
            await table.ReloadServerData();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync("Unidad Resdencial Eliminada", string.Empty, SweetAlertIcon.Success);
        }
    }
}