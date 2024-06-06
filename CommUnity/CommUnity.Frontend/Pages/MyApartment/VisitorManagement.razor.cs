using Blazored.Modal;
using Blazored.Modal.Services;
using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Pages.MyApartment
{


    public partial class VisitorManagement
    {
        private VisitorEntryDTO visitorEntryDTO = new();

        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [CascadingParameter] IModalService Modal { get; set; } = default!;
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;


        private void ScheduleVisitorModal()
        {
            Modal.Show<ScheduleVisitor>();
        }
        private void ListVisitorModal()
        {
            Modal.Show<Listvisitors>();
        }
        private async Task Return()
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok());
        }
    }




}
