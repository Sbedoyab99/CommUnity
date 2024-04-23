using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.DTOs
{
    public class ApartmentDTO
    {
        public int Id { get; set; }

        [Display(Name = "Apartamento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Number { get; set; } = null!;

        public int ResidentialUnitId { get; set; }
    }
}
