using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.DTOs
{
    public class MailArrivalDTO
    {
        public int? Id { get; set; }

        [Display(Name = "Remitente")]
        public string Sender { get; set; } = null!;

        [Display(Name = "Tipo")]
        public string? Type { get; set; }

        [Display(Name = "Estado")]
        public MailStatus Status { get; set; }

        [Display(Name = "Apartamento")]
        public int ApartmentId { get; set; }
    }
}
