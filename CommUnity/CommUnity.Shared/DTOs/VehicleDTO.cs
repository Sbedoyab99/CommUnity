using System.ComponentModel.DataAnnotations;


namespace CommUnity.Shared.DTOs
{
    public class VehicleDTO
    {
        public int Id { get; set; }

        [Display(Name = "Vehiculo")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Plate { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;


        public int ApartmentId { get; set; }
        
    }
}
