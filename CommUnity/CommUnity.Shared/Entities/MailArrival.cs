using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class MailArrival : Event
    {
        public string? Sender { get; set; }
        public DateTime? Date { get; set; }
        public string? Type { get; set; }
    }
}
