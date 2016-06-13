using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.EntityFramework;

namespace CarRentalWebSite.Models
{
    public class CarDetailsViewModel
    {
        //public int Id { get { return Car.Id; } }

        public Car Car { get; set; }

        public Car NextCar { get; set; }
        public Car PrevCar { get; set; }

        public Reservation Reservation { get; set; }

        // Number of stars
        public double AverageRating
        {
            get { return Car.Reviews.Any() ? Car.Reviews.Average(review => review.Rating) : 0; }
        }

        public double RatingStars
        {
            get { return Convert.ToInt32(AverageRating); }
        }

        public List<CarDetail> Specification { get { return Car.CarDetails.ToList(); } }

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
        //public List<Comment> CarComments = new List<Comment>();

        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        // Used to switch between 'Provjeri dostupnost' i 'Rezerviraj' buttons
        public bool IsAvailable { get; set; }

        public CarDetailsViewModel()
        {
        }

        public CarDetailsViewModel(Car car)
        {
            Init(car);
        }

        public CarDetailsViewModel(Car car, Reservation reservation)
        {
            Init(car);
            this.Reservation = reservation;
        }

        private void Init(Car car)
        {
            this.Car = car;

            //this.Specification = car.CarDetails.ToList();

            //foreach (var review in this.Car.Reviews.Where(review => !string.IsNullOrEmpty(review.Comment)))
            //{
            //    this.CarComments.Add(new Comment(null, review.Comment));
            //}
        }
    }
}