using System.ComponentModel;

namespace CommUnity.Shared.Enums
{
    public enum UserType
    {
        [Description("Administrador")]
        Admin,

        [Description("Aministrador Unidad Residencial")]
        AdminResidentialUnit,

        [Description("Trabajador")]
        Worker,

        [Description("Residente")]
        Resident
    }
}