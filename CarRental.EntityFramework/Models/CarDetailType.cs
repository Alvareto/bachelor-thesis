using System.ComponentModel.DataAnnotations;

namespace CarRental.EntityFramework
{
    public enum CarDetailType
    {
        [Display(Name="Kapacitet spremnika za gorivo")]
        GasTankCapacity, 
        EnginePower, 
        FuelConsumption
    }
}