using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class Pqrs
    {
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Tipo")]
        [Range(0, int.MaxValue, ErrorMessage = "Debes seleccionar un {0}.")]
        public PqrsType PqrsType { get; set; }

        [Display(Name = "Contenido")]
        [MaxLength(3000, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Content { get; set; }

        public PqrsState PqrsState { get; set; }

        public int ApartmentId { get; set; }

        public Apartment? Apartment { get; set; }

        public int ResidentialUnitId { get; set; }

        public ResidentialUnit? ResidentialUnit { get; set; }

        public ICollection<PqrsMovement>? PqrsMovements { get; set; }

        [Display(Name = "Pqrs Movimientos")]
        public int PqrsMovementsNumber => PqrsMovements == null || PqrsMovements.Count == 0 ? 0 : PqrsMovements.Count;

    }
}
