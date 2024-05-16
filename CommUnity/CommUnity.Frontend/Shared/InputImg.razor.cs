using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Shared
{
    public partial class InputImg
    {
        private string? imageBase64;

        [Parameter] public string Label { get; set; } = "Imagen";
        [Parameter] public string? ImageURL { get; set; }
        [Parameter] public EventCallback<string> ImageSelected { get; set; }

        private async Task OnChange(IBrowserFile file)
        {
            if(file != null)
            {
                var arrBytes = new byte[file.Size];
                await file.OpenReadStream().ReadAsync(arrBytes);
                imageBase64 = Convert.ToBase64String(arrBytes);
                ImageURL = null;
                await ImageSelected.InvokeAsync(imageBase64);
            }
        }
    }
}