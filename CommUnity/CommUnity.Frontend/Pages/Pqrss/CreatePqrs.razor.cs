using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Pqrss
{
    public partial class CreatePqrs
    {
        private PqrsDTO pqrsDTO = new();
        public bool loading { get; set; }

        [Parameter] public int ApartmentId { get; set; }
        [Parameter] public int ResidentialUnitId { get; set; }

        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] public IRepository Repository { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private PqrsType Type;//= PqrsType.Request;

        private List<PqrsType> types = new List<PqrsType>
        {
            PqrsType.Request,
            PqrsType.Complaint,
            PqrsType.Claim,
            PqrsType.Suggestion
        };

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

        private async Task CreatePqrsAsync()
        {
            loading = true;

            var psqr = new PqrsDTO()
            {
                DateTime = DateTime.Now,
                Type = pqrsDTO.Type,
                Content = pqrsDTO.Content,
                Status = PqrsState.Settled,
                ApartmentId = ApartmentId,
                ResidentialUnitId = ResidentialUnitId

            };

            var responseHttp = await Repository.PostAsync("/api/Pqrss/create", psqr);
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
            //MudDialog.Close();
            MudDialog.Close(DialogResult.Ok(true));
            await toast.FireAsync(new SweetAlertOptions
            {
                Title = "PQRS registrada con exito.",
                Icon = SweetAlertIcon.Success,
            });
            return;
        }

        private void ChangedValueType(PqrsType pqrsType)
        {
            Type = pqrsType;
            pqrsDTO.Type = pqrsType;
        }

        private void Return()
        {
            MudDialog.Close(DialogResult.Cancel());
        }
    }
}