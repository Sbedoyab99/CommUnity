using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Enums
{
    public enum ReservationStatus
    {
        [Description("Pendiente")]
        Pending,
        [Description("Aprobada")]
        Approved,
        [Description("Rechazada")]
        Rejected,
        [Description("Cancelada")]
        Canceled
    }
}
