using System.ComponentModel.DataAnnotations;


namespace CommUnity.Shared.DTOs
{
    public class CommonZoneDTO
    {
        public int Id { get; set; }

        [Display(Name = "Zona Común")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public int ResidentialUnitId { get; set; }

        public int Capacity { get; set; }
    }
}
