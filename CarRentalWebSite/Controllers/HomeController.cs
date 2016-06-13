using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRental.EntityFramework;
using CarRentalWebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRentalWebSite.Controllers
{
    public class HomeController : Controller
    {
        private CarRentalWebSiteContext db = new CarRentalWebSiteContext();
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        public ActionResult Index(int? id)
        {
            if (id == null)
            {

            }
            ViewBag.Available = false;
            return View(new CarDetailsViewModel(db.CarSet.FirstOrDefault()));
        }

        public ActionResult Check([Bind(Include = "Id,DateStarted,DateEnded")] Reservation reservation, int? carId)
        {
            if (carId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Car is not set");
            }
            Car car = db.CarSet.Find(carId);

            if (reservation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Reservation is not set");
            }
            if (reservation.DateStarted.Date > reservation.DateEnded.Date)
            {
                ModelState.AddModelError("", "Datum početka rezervacije mora biti prije datuma kraja rezervacije");
            }
            if (reservation.DateStarted.Date < DateTime.Today.Date)
            {
                ModelState.AddModelError("", "Datum početka rezervacije mora biti u budućnosti");
            }

            ViewBag.Available = Available(reservation, car); ;

            return View("Index", new CarDetailsViewModel(car, reservation));
        }


        private Boolean Available(Reservation reservation, Car car)
        {
            // cars -> reservation dates overlap ? -> return if not
            HashSet<Car> set = new HashSet<Car>();
            //List<Car> list = new List<Car>();
            foreach (var r in car.Reservations.Where(res => !res.Canceled))
            {
                //if (!DateRangesOverlap(reservation.DateStarted, reservation.DateEnded, r.DateStarted, r.DateEnded))
                //    set.Add(car);
                // if for any reservation we have overlap, car is not available
                if (DateRangesOverlap(reservation.DateStarted, reservation.DateEnded, r.DateStarted, r.DateEnded))
                    return false;
            }
            return true;
        }

        private bool DateRangesOverlap(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
        {
            //bool overlap = a.start < b.end && b.start < a.end;
            return aStart.Date <= bEnd.Date && bStart.Date <= aEnd.Date;
        }


        public ActionResult Reserve(int carId, DateTime started, DateTime ended)
        {
            Reservation reservation = new Reservation
            {
                Client_Id = User.Identity.GetUserId(),
                DateStarted = started,
                DateEnded = ended,
                Car = db.CarSet.Find(carId),
                Canceled = false
            };
            //var o = (reservation.DateEnded - reservation.DateStarted).Days;
            db.ReservationSet.Add(reservation);
            db.SaveChanges();

            return RedirectToAction("Details", "Reservations", new { id = reservation.Id });
        }



























        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}