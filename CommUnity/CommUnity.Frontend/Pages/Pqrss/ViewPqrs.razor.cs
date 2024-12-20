using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Pqrss
{
    public partial class ViewPqrs
    {
        private Pqrs? pqrs;
        public bool loading { get; set; }
        [Parameter] public int Id { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private PqrsType Type;//= PqrsType.Request;
        private PqrsState Status;// = PqrsState.Settled;

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

        protected override async Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Pqrs>($"api/Pqrss/{Id}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                Return();
            }
            else
            {
                pqrs = responseHttp.Response!;
                ChangedValueType(responseHttp.Response!.PqrsType);
                ChangedValueStatus(responseHttp.Response!.PqrsState);
            }
        }

        private IEnumerable<string> MaxCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 2999 < ch?.Length)
                yield return "Max 2999 characters";
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

        private void ChangedValueType(PqrsType pqrsType)
        {
            Type = pqrsType;
            pqrs!.PqrsType = pqrsType;
        }

        private void ChangedValueStatus(PqrsState pqrsState)
        {
            Status = pqrsState;
            pqrs!.PqrsState = pqrsState;
        }

        private void Return()
        {
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}