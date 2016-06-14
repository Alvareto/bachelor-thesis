namespace CarRental.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReviewSet")]
    public partial class Review
    {
        public int Id { get; set; }

        [Display(Name = "Ocjena")]
        public int Rating { get; set; }

        [Display(Name = "Komentar o iskustvu vožnje", ShortName = "Komentar")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Display(Name = "Automobil")]
        public int Car_Id { get; set; }

        [Display(Name = "Rezervacija")]
        public int? Reservation_Id { get; set; }

        public virtual Car Car { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
