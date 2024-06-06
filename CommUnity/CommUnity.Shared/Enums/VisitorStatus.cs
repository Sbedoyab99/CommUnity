using System.ComponentModel;

namespace CommUnity.Shared.Enums
{
    public enum VisitorStatus
    {
        [Description("Programado")]
        Scheduled,
        [Description("Aprobado")]
        Approved,
        [Description("Cancelado")]
        Canceled
    }
}
