using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Pqrss
{
    public partial class UpdateStatusPqrs
    {
        private PqrsDTO pqrsDTO = new();
        public bool loading { get; set; }

        [Parameter] public int Id { get; set; }
        [Parameter] public List<PqrsState> AvailableStates { get; set; } = new List<PqrsState>();
        [Parameter] public PqrsState CurrentState { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private PqrsState Status;

        private List<PqrsState> states = new List<PqrsState>
        {
            PqrsState.Settled,
            PqrsState.InReview,
            PqrsState.InProgress,
            PqrsState.Resolved,
            PqrsState.Closed
        };

        protected override Task OnParametersSetAsync()
        {
            ChangedValueStatus(CurrentState + 1);
            return Task.CompletedTask;
        }

        private IEnumerable<string> MaxCharacters(string ch)
        {
            if (!string.IsNullOrEmpty(ch) && 2999 < ch?.Length)
                yield return "Max 2999 characters";
        }

        private async Task<IEnumerable<PqrsState>> SearchState(string searchText)
        {
            await Task.Delay(5);
            //return states!;

            int currentStateIndex = states.IndexOf(CurrentState);
            return states
                .Where(state => states.IndexOf(state) > currentStateIndex)
                .Where(state => state.ToString().Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }

        private void ChangedValueStatus(PqrsState status)
        {
            Status = status;
            pqrsDTO.Status = Status;
        }

        private void Return()
        {
            MudDialog.Close(DialogResult.Cancel());
        }

        private async Task UpdateStatusPqrsAsync()
        {

            var pqrs = new PqrsDTO()
            {
                Id = Id,
                Status = pqrsDTO.Status,
                Content = "Default Content",
                Observation = pqrsDTO.Observation
            };

            var responseHttp = await Repository.PutAsync("api/Pqrss/updateStatusPqrs", pqrs);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Error",
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                });
                return;
            }
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            MudDialog.Close(DialogResult.Ok(true));
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "Estado Actualizado",
                Icon = SweetAlertIcon.Success,
            });
        }
    }
}