using System.ComponentModel;

namespace CarRental.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReservationSet")]
    public partial class Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservation()
        {
            ReviewSet = new HashSet<Review>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Poèetak")]
        [DataType(DataType.Date)]
        public DateTime DateStarted { get; set; }

        [Display(Name = "Kraj")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateEnded { get; set; }

        [Display(Name = "Otkazana?")]
        public Boolean Canceled { get; set; }

        public int Car_Id { get; set; }

        /// <summary>
        /// This is used to retrieve a Application User Identity from external database.
        /// It should be set on Reservation creation.
        /// </summary>
        [Display(Name = "Korisnik")]
        [Required]
        public string Client_Id { get; set; }

        [Display(Name = "Automobil")]
        public virtual Car Car { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> ReviewSet { get; set; }
    }
}
