using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class VisitorEntry : Event
    {
        [Display(Name = "Nombre")]
        public string? Name { get; set; }

        public string? Plate { get; set; }

        public VisitorStatus? Status { get; set; }
    }
}
