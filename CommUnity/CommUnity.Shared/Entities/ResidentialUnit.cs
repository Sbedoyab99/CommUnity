using System.ComponentModel.DataAnnotations;

namespace CommUnity.Shared.Entities
{
    public class ResidentialUnit
    {
        public int Id { get; set; }

        [Display(Name = "Unidad Residencial")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int CityId { get; set; }

        public City? City { get; set; }

        public ICollection<Apartment>? Apartments { get; set; }

        [Display(Name = "Apartamentos")]
        public int ApartmentsNumber => Apartments == null || Apartments.Count == 0 ? 0 : Apartments.Count;

        public ICollection<CommonZone>? CommonZones { get; set; }

        [Display(Name = "Zonas Comunes")]
        public int CommonZonesNumber => CommonZones == null || CommonZones.Count == 0 ? 0 : CommonZones.Count;

       public ICollection<News>? News { get; set; }

       [Display(Name = "Noticias")]
        public int NewsNumber => News == null || News.Count == 0 ? 0 : News.Count;
    }
}