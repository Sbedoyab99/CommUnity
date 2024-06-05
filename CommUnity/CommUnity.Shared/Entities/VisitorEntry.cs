using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class VisitorEntry : Event
    {
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string? Plate { get; set; }
        public VisitorStatus? Status { get; set; }
    }
}
