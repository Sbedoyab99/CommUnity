using CommUnity.FrontEnd.Repositories;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.MyResidentialUnit
{
    public partial class NewWorkerForm
    {
        private UserDTO userDTO = new();
        private bool loading;

        [Parameter] public ResidentialUnit? ResidentialUnit { get; set; }

        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        private async Task CreateUserAsync()
        {
            loading = true;
            userDTO.ResidentialUnitId = ResidentialUnit?.Id;
            userDTO.CityId = ResidentialUnit.CityId!;
            userDTO.UserType = UserType.Worker;
            userDTO.PasswordConfirm = userDTO.Password;
            userDTO.Address = ResidentialUnit.Address;
            userDTO.UserName = userDTO.Email;

            var responseHttp = await Repository.PostAsync<UserDTO>("/api/accounts/CreateUser", userDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            MudDialog.Close();
            await SweetAlertService.FireAsync("Confirmaci�n", "Lu cuenta ha sido creada con exito. Se enviara un correo electronico con las instrucciones iniciar sesi�n.", SweetAlertIcon.Info);
        }

        private void Return()
        {
            MudDialog.Close();
        }
    }
}