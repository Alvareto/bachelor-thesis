namespace CarRental.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarDetailSet")]
    public partial class CarDetail
    {
        public int Id { get; set; }

        public CarDetailType Type { get; set; }

        [Required]
        public string Value { get; set; }

        public int Car_Id { get; set; }

        public virtual Car Car { get; set; }

    }
}
