
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace CommUnity.FrontEnd.Pages.Newss
{
    public partial class NewsForm
    {
        private EditContext editContext = null!;
        private string? imageUrl;

        [Parameter, EditorRequired] public News news { get; set; } = null!;
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        [Parameter] public EventCallback AddImageAction { get; set; }
        [Parameter] public EventCallback RemoveImageAction { get; set; }

        public bool FormPostedSuccesfully { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(news!);
        }

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(news.Picture))
            {
                imageUrl = news.Picture;
                news.Picture = null;
            }
        }

        private void ImageSelected(string imagenBase64)
        {
            if (news.Picture is null)
            {
                news.Picture = "";
            }

            news.Picture = imagenBase64;
            imageUrl = null;
        }

        private async Task OnDateChange(DateTime? date)
        {
            await Task.Delay(1);
            if (date == null)
            {
                return;
            }
            news.Date = (DateTime)date;
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