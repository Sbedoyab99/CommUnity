﻿using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Shared
{
    public partial class Filter
    {
        [Parameter] public string FilterValue { get; set; } = string.Empty;
        [Parameter] public EventCallback<string> ApplyFilter { get; set; }

        private async Task CleanFilter()
        {
            FilterValue = string.Empty;
            await ApplyFilter.InvokeAsync(FilterValue);
        }

        private async Task OnFilterApply()
        {
            await ApplyFilter.InvokeAsync(FilterValue);
        }
    }
}
