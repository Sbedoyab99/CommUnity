using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Shared
{
    public partial class LoadingOverlay
    {
        [Parameter] public bool IsLoading { get; set; }
    }
}

