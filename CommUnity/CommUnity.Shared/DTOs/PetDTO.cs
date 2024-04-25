using System.ComponentModel.DataAnnotations;


namespace CommUnity.Shared.DTOs
{
    public class PetDTO
    {
        public int Id { get; set; }

        [Display(Name = "Mascotas")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public string Picture { get; set; } = null!;

        public int ApartmentId { get; set; }
        
    }
}
