using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bo_Voyage_Final.Models
{
    public partial class Voyage //: IValidatableObject
    {
        public Voyage()
        {
            Dossierresa = new HashSet<Dossierresa>();
            Voyageur = new HashSet<Voyageur>();
        }

        [Display(Name = "Id voyage")]
        public int Id { get; set; }
        [Display(Name = "Destination")]
        public int IdDestination { get; set; }

        [Display(Name = "Date de départ"), DataType(DataType.Date)]
       // [Required(ErrorMessage ="La date de départ doit être precisée ")]
        public DateTime DateDepart { get; set; }
        [Display(Name = "Date de Retour"), DataType(DataType.Date)]
        //[Required(ErrorMessage ="La date de retour doit être precisée ")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
        //          ApplyFormatInEditMode = true)]
        public DateTime DateRetour { get; set; }

        [Display(Name = "Places disponibles")]
        //[Required(ErrorMessage ="Le nombre de places doit être précisé")]
        [Range(1,9999,ErrorMessage ="Le nombre de places doit être positif et plus grand que 1")]

        public int PlacesDispo { get; set; }
        [Display(Name = "Prix hors-taxe par pers.")]
        [DataType(DataType.Currency)]
        public decimal PrixHt { get; set; }
        [Display(Name = "Réduction")]
        //[Range(0.0,1.0,ErrorMessage ="La reduction doit être entre 0 et 1")]
        //[Required(ErrorMessage ="La réduction doit être précisée")]
        public decimal Reduction { get; set; }
        [Required(ErrorMessage ="il faut un text")]
        public string Descriptif { get; set; }
        [Display(Name = "Destination")]
        public virtual Destination IdDestinationNavigation { get; set; }
        public virtual ICollection<Dossierresa> Dossierresa { get; set; }
        public virtual ICollection<Voyageur> Voyageur { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    Voyage v = (Voyage)validationContext.ObjectInstance;
        //    if (v.DateDepart > v.DateRetour)
        //    {
        //        yield return new ValidationResult("La date de retour doit être postérieure à la date de départ!", new string[] { "DateRetour" ,"DateDepart"});
        //    }
        //}
    }
}
