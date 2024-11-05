using CommUnity.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommUnity.Shared.Entities
{
    public class PqrsMovement
    {
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Observación")]
        [MaxLength(3000, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Observation { get; set; } = null!;

        public PqrsState PqrsState { get; set; }

        public int PqrsId { get; set; }

        public Pqrs? Pqrs { get; set; }
    }
}
