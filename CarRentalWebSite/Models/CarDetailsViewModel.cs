using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.EntityFramework;

namespace CarRentalWebSite.Models
{
    public class CarDetailsViewModel
    {
        public int Id { get; set; }

        public Car Car { get; set; }

        // Number of stars
        public Int32 AverageRating { get; set; }
        public String CarManufacturer { get; set; }
        public String CarModel { get; set; }

        public List<CarDetail> Specification { get; set; }

        public class Comment
        {
            public String UserName;
            public String Value;

            public Comment(String name, String value)
            {
                this.UserName = name;
                this.Value = value;
            }
        }
        public List<Comment> CarComments = new List<Comment>();

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        // Used to switch between 'Provjeri dostupnost' i 'Rezerviraj' buttons
        public bool IsAvailable { get; set; }

        public CarDetailsViewModel(Car car)
        {
            this.Car = car;
            this.Id = this.Car.Id;

            this.CarManufacturer = this.Car.Manufacturer;
            this.CarModel = this.Car.Model;
            this.Specification = car.CarDetails.ToList();

            var reviews = this.Car.Reviews;
            if (reviews.Any())
                this.AverageRating = Convert.ToInt32(reviews.Average(review => review.Rating));
            foreach (var review in this.Car.Reviews.Where(review => !string.IsNullOrEmpty(review.Comment)))
            {
                this.CarComments.Add(new Comment(null, review.Comment));
            }
        }
    }
}