using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Pages.Auth;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.MyApartment
{


    public partial class VisitorManagement
    {
        [Parameter] public int ApartmentId { get; set; }

        [Inject] private IDialogService DialogService { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private void ScheduleVisitorModal()
        {
            var parameters = new DialogParameters<ScheduleVisitor> { { x => x.ApartmentId, ApartmentId } };
            DialogService.Show<ScheduleVisitor>("Programar Visitante", parameters);
        }

        private void ListVisitorModal()
        {
            var parameters = new DialogParameters<Listvisitors> { { x => x.ApartmentId, ApartmentId } };
            DialogService.Show<Listvisitors>("Lista de Visitantes",parameters);
        }

        private void Return()
        {
            MudDialog.Cancel();
        }
    }
}
