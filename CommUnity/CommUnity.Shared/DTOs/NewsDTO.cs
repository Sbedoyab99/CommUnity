using System.ComponentModel.DataAnnotations;


namespace CommUnity.Shared.DTOs
{
    public class NewsDTO
    {
        public int Id { get; set; }

        [Display(Name = "Noticias")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Picture { get; set; } = null!;

        public int ResidentialUnitId { get; set; }
        
    }
}
