using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class Pet
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Breed { get; set; } = null!;

        public string? Picture { get; set; }

        public int ApartmentId { get; set; }

        public Apartment? Apartment { get; set; }
    }
}