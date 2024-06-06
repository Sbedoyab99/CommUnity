using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    [JsonDerivedType(typeof(Event), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(VisitorEntry), typeDiscriminator: "visitor")]
    [JsonDerivedType(typeof(MailArrival), typeDiscriminator: "mail")]
    [JsonDerivedType(typeof(CommonZoneReservation), typeDiscriminator: "reservation")]
    public class Event
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
