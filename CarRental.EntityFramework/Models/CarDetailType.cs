using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarRental.EntityFramework
{
    public enum CarDetailType
    {
        [Description("Kapacitet spremnika za gorivo (L)")]
        GasTankCapacity,
        [Description("Snaga motora (KS)")]
        EnginePower,
        [Description("Potrošnja goriva (L/100km)")]
        FuelConsumption
    }
}