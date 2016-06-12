using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.EntityFramework;

namespace CarRentalWebSite.Models
{
    public class CarSpecificationViewModel
    {
        public CarDetail GasTankCapacity { get; set; }
        public CarDetail EnginePower { get; set; }
        public CarDetail FuelConsumption { get; set; }




        public int Id { get; set; }

        public Car Car { get; set; }

        public Office Office { get; set; }

        public List<CarDetail> Specification { get; set; }

        public CarSpecificationViewModel(Car car, Office o)
        {
            this.Car = car;

            this.Id = this.Car.Id;
            this.Office = o;
            this.Specification = car.CarDetails.ToList();

        }

        public CarSpecificationViewModel(Car car)
        {
            throw new NotImplementedException();
        }

        public CarSpecificationViewModel()
        {
            throw new NotImplementedException();
        }
    }
}