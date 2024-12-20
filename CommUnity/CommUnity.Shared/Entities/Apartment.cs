﻿using System.ComponentModel.DataAnnotations;

namespace CommUnity.Shared.Entities
{
    public class Apartment
    {
        public int Id { get; set; }

        [Display(Name = "Apartamento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Number { get; set; } = null!;

        public int ResidentialUnitId { get; set; }

        public ResidentialUnit? ResidentialUnit { get; set; }

        public ICollection<Vehicle>? Vehicles { get; set; }

        [Display(Name = "Vehículos")]
        public int VehiclesNumber => Vehicles == null || Vehicles.Count == 0 ? 0 : Vehicles.Count;

        public ICollection<Pet>? Pets { get; set; }

        [Display(Name = "Mascotas")]
        public int PetsNumber => Pets == null || Pets.Count == 0 ? 0 : Pets.Count;

        public ICollection<User>? Users { get; set; }

        [Display(Name = "Usuarios")]
        public int UsersNumber => Users == null || Users.Count == 0 ? 0 : Users.Count;

        public ICollection<Event>? Events { get; set; }

        [Display(Name = "Eventos")]
        public int EventsNumber => Events == null || Events.Count == 0 ? 0 : Events.Count;

        public ICollection<Pqrs>? Pqrss { get; set; }

        [Display(Name = "PQRS")]
        public int PqrsNumber => Pqrss == null || Pqrss.Count == 0 ? 0 : Pqrss.Count;

    }
}