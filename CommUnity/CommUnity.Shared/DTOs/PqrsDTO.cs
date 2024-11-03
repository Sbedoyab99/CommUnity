using CommUnity.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommUnity.Shared.DTOs
{
    public class PqrsDTO
    {

        public int? Id { get; set; }

        [Display(Name = "Fecha")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Debes seleccionar un {0}.")]
        public PqrsType Type { get; set; }

        [Display(Name = "Contenido")]
        [MaxLength(3000, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Content { get; set; } = null!;

        [Display(Name = "Estado")]
        public PqrsState Status { get; set; }

        [Display(Name = "Apartamento")]
        public int ApartmentId { get; set; }

        [Display(Name = "Unidad Residencial")]
        public int ResidentialUnitId { get; set; }

    }
}
