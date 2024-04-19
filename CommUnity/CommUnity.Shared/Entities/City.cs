using CommUnity.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class City : IEntityWithName
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public int StateId { get; set; }

        public State? State { get; set; }

        public ICollection<ResidentialUnit>? ResidentialUnits { get; set; }

        [Display(Name = "Unidades Residenciales")]
        public int ResidentialUnitsNumber => ResidentialUnits == null || ResidentialUnits.Count == 0 ? 0 : ResidentialUnits.Count;
    }
}