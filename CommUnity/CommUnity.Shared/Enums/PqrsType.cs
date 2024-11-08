using System.ComponentModel;

namespace CommUnity.Shared.Enums
{
    public enum PqrsType
    {

        [Description("Petición")]
        Request,
        [Description("Queja")]
        Complaint,
        [Description("Reclamo")]
        Claim,
        [Description("Sugerencia")]
        Suggestion
    }
}

