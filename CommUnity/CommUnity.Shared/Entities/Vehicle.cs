using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }

        public string Plate { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int ApartmentId { get; set; }

        public Apartment? Apartment { get; set; }
    }
}