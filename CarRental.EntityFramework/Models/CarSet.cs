using System.ComponentModel;

namespace CarRental.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarSet")]
    [DisplayName("Automobil")]
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            CarDetails = new HashSet<CarDetail>();
            Reservations = new HashSet<Reservation>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Proizvoðaè")]
        public string Manufacturer { get; set; }

        [Display(Name = "Model")]
        [Required]
        public string Model { get; set; }

        public int Office_Id { get; set; }

        [Display(Name = "Cijena")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }

        public double GetPriceOffSeason()
        {
            if (Office == null)
                return Price;
            else
                return Price * (100 - Office.Discount) / 100;
        }

        /// <summary>
        /// Returns price based on on/off-season logic.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public double GetReservationPrice(DateTime date)
        {
            int month = date.Month;
            if (month > 5 && month < 10)
                return GetPriceOffSeason();
            else return Price;
        }

        public override string ToString()
        {
            return this.Manufacturer + " " + this.Model;
        }

        public string FullName
        {
            get { return this.ToString(); }
            private set { }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarDetail> CarDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }

        public virtual Office Office { get; set; }

    }
}
