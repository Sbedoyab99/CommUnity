using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
