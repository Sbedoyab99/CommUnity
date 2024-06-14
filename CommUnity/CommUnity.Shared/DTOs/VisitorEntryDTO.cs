using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.DTOs
{
    public class VisitorEntryDTO
    {
        public int? Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Display(Name = "Placa")]
        public string? Plate { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Estado")]
        public VisitorStatus Status { get; set; }

        [Display(Name = "Apartamento")]
        public int ApartmentId { get; set; }
    }
}
