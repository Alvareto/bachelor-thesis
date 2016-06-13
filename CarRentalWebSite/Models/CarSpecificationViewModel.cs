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
        public Car Car { get; set; }
        public CarSpecificationViewModel(Car car)
        {

            GasTankCapacity = car.CarDetails.Any(o => o.Type == CarDetailType.GasTankCapacity) ?
                car.CarDetails.Single(detail => detail.Type == CarDetailType.GasTankCapacity)
                : new CarDetail()
                {
                    Car = null,
                    Type = CarDetailType.GasTankCapacity,
                    Value = ""
                };
            EnginePower = car.CarDetails.Any(o => o.Type == CarDetailType.EnginePower) ?
                car.CarDetails.Single(detail => detail.Type == CarDetailType.EnginePower)
                : new CarDetail()
                {
                    Car = null,
                    Type = CarDetailType.EnginePower,
                    Value = ""
                };
            FuelConsumption = car.CarDetails.Any(o => o.Type == CarDetailType.FuelConsumption) ?
                car.CarDetails.Single(detail => detail.Type == CarDetailType.FuelConsumption)
                : new CarDetail()
                {
                    Car = null,
                    Type = CarDetailType.FuelConsumption,
                    Value = ""
                };
        }
        public CarSpecificationViewModel()
        {
            GasTankCapacity = new CarDetail()
            {
                Car = null,
                Type = CarDetailType.GasTankCapacity,
                Value = ""
            };
            EnginePower = new CarDetail()
            {
                Car = null,
                Type = CarDetailType.EnginePower,
                Value = ""
            };
            FuelConsumption = new CarDetail()
            {
                Car = null,
                Type = CarDetailType.FuelConsumption,
                Value = ""
            };
        }
    }
}