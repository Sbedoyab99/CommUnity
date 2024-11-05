using System.ComponentModel;

namespace CommUnity.Shared.Enums
{
    public enum PqrsState
    {
        [Description("Radicada")]
        Settled,
        [Description("En Revisión")]
        InReview,
        [Description("En Progeso")]
        InProgress,
        [Description("Resuelta")]
        Resolved,
        [Description("Cerrada")]
        Closed
    }
}

