using System;
using System.Net;
using System.Web.Mvc;
using CarRental.EntityFramework;

namespace CarRentalWebSite.Controllers
{
    public class ClientController : Controller
    {
        private CarRentalWebSiteContext db = new CarRentalWebSiteContext();

        public ActionResult CheckAvailability(int id, DateTime @from, DateTime to)
        {
            Car car = db.CarSet.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return null;
        }

        public ActionResult MakeReservation(int id, DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public ActionResult Previous(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Next(int id)
        {
            throw new NotImplementedException();
        }
    }
}