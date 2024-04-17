using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class Apartment
    {
        public int Id { get; set; }

        [Display(Name = "Apartamento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Number { get; set; } = null!;

        public int ResidentialUnitId { get; set; }

        public ResidentialUnit? ResidentialUnit { get; set; }

        public ICollection<Vehicle>? Vehicles { get; set; }

        [Display(Name = "Vehículos")]
        public int VehiclesNumber => Vehicles == null || Vehicles.Count == 0 ? 0 : Vehicles.Count;

        public ICollection<Pet>? Pets { get; set; }

        [Display(Name = "Mascotas")]
        public int PetsNumber => Pets == null || Pets.Count == 0 ? 0 : Pets.Count;
    }
}