using Microsoft.AspNetCore.Components;

namespace CommUnity.FrontEnd.Shared
{
    public partial class GenericList<TItem>
    {
        [Parameter] public RenderFragment? Loading { get; set; }

        [Parameter] public RenderFragment? NoRecords { get; set; }

        [EditorRequired, Parameter] public RenderFragment? Body { get; set; }

        [EditorRequired, Parameter] public List<TItem>? MyList { get; set; }
    }
}