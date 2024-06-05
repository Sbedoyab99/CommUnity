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
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [Display(Name = "Placa")]
        public string? Plate { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
    }
}
