using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public abstract class Event
    {
        public int Id { get; set; }

        public string? EventType { get; set; }

        [Display(Name = "Fecha")]
        public DateTime DateTime { get; set; }

        public int? ResidentialUnitId { get; set; }

        public ResidentialUnit? ResidentialUnit { get; set; }

        public int? ApartmentId { get; set; }

        public Apartment? Apartment { get; set; }
    }
}
