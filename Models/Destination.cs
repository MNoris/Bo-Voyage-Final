using System;
using System.Collections.Generic;

namespace Bo_Voyage_Final.Models
{
    public partial class Destination
    {
        public Destination()
        {
            InverseIdParenteNavigation = new HashSet<Destination>();
            Photo = new HashSet<Photo>();
            Voyage = new HashSet<Voyage>();
        }

        public int Id { get; set; }
        public int IdParente { get; set; }
        public string Nom { get; set; }
        public byte Niveau { get; set; }
        public string Description { get; set; }

        public virtual Destination IdParenteNavigation { get; set; }
        public virtual ICollection<Destination> InverseIdParenteNavigation { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
        public virtual ICollection<Voyage> Voyage { get; set; }
    }
}
