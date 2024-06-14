using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class CommonZoneReservation : Event
    {
        public int CommonZoneId { get; set; }

        public CommonZone? CommonZone { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
