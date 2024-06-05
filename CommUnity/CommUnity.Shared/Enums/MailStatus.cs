using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Enums
{
    public enum MailStatus
    {
        [Display(Name = "Entregado")]
        Delivered,
        [Display(Name = "Almacenado")]
        Stored
    }
}
