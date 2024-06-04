using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace CommUnity.FrontEnd.Pages.Pets
{
    public partial class PetForm
    {
        private EditContext editContext = null!;
        private string? imageUrl;
        [EditorRequired, Parameter] public Pet Pet { get; set; } = default!;
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        public bool FormPostedSuccesfully { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(Pet!);
        }
        protected override void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Pet.Picture))
            {
                imageUrl = Pet.Picture;
                Pet.Picture = null;
            }
        }
        private void ImageSelected(string imagenBase64)
        {
            if (Pet.Picture is null) 
            {
            Pet.Picture = null;
            }
            Pet.Picture = imagenBase64;
            imageUrl = null;
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formWasEdited = editContext.IsModified();
            if (!formWasEdited || FormPostedSuccesfully)
            {
                return;
            }
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = "¿Deseas abandonar la pagina y perder los cambios?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (!confirm)
            {
                return;
            }
            context.PreventNavigation();
        }
    }
}