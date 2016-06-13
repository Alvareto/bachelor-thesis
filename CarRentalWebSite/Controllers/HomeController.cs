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



        public ActionResult Index(int? officeId, int? carId)
        {
            Car car;

            // carId != null && office whatever => car
            if (carId != null)
            {
                car = db.CarSet.Find(carId);
            }
            else
            {
                // carId == null && officeId == null  => first car in first office
                if (officeId == null)
                {
                    var officeFirst = db.OfficeSet.FirstOrDefault();
                    if (officeFirst != null)
                        officeId = officeFirst.Id;
                }

                // carId == null && officeId != null => first car in office
                car = db.CarSet.FirstOrDefault(o => o.Office.Id == officeId);
            }

            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);

            // previousCar, nextCar

            SetNextAndPrevCars(car.Id);

            var model = new CarDetailsViewModel(car, new Reservation())
            {
                NextCar = nextCarId != 0 ? db.CarSet.Find(nextCarId) : null,
                PrevCar = prevCarId != 0 ? db.CarSet.Find(prevCarId) : null,
                IsAvailable = ViewBag.Available = false
            };

            //ViewBag.Available = false;


            return View(model);
        }

        [HttpPost]
        public ActionResult Check(DateTime DateStarted, DateTime DateEnded, int carId, int? officeId)
        {
            if (DateStarted.Date > DateEnded.Date)
            {
                ModelState.AddModelError("", "Datum početka rezervacije mora biti prije datuma kraja rezervacije");
            }
            if (DateStarted.Date < DateTime.Today.Date)
            {
                ModelState.AddModelError("", "Datum početka rezervacije mora biti u budućnosti");
            }

            Car car = db.CarSet.Find(carId);
            Reservation reservation = new Reservation()
            {
                DateStarted = DateStarted,
                DateEnded = DateEnded,
                Canceled = false,
                Car = car,
                Client_Id = User.Identity.GetUserId()
            };

            SetNextAndPrevCars(carId);
            var model = new CarDetailsViewModel(car, reservation)
            {
                NextCar = nextCarId != 0 ? db.CarSet.Find(nextCarId) : null,
                PrevCar = prevCarId != 0 ? db.CarSet.Find(prevCarId) : null,
                IsAvailable = Available(reservation, car)
            };
            ViewBag.Available = model.IsAvailable; // = Available(model.Reservation, model.Car);

            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);

            return View("Index", model);
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



        public ActionResult Cancel(int carId)
        {
            ViewBag.Available = false;
            return RedirectToAction("Index", new { carId = carId });
        }

        public ActionResult ChangeCity(int officeId)
        {
            return RedirectToAction("Index", new { officeId });
        }


        private int prevCarId = 0;


        private int nextCarId = 0;

        /// <summary>
        /// Sets <c>IDs</c> for next and previous Car entries.
        /// </summary>
        /// <param name="id">ID of current Car entry.</param>
        private bool SetNextAndPrevCars(int id)
        {
            var car = db.CarSet.Find(id);

            // News doesn't exists.
            if (car == null)
            {
                return false;
            }

            //// Expired news - show Details without PREV and NEXT
            //if (car.DatumIsteka < DateTime.Now || isLoggedIn)
            //{
            //    return true;
            //}

            var carId = db.CarSet.Where(n => n.Price > 0)
                                  .OrderBy(n => n.Id)
                                  .Select(n => new { n.Id }).ToArray();

            for (int i = 0, N = carId.Length; i < N; i++)
            {
                if (carId[i].Id == id)
                {
                    if (i > 0)
                    {
                        nextCarId = carId[i - 1].Id;
                    }

                    if (i < N - 1)
                    {
                        prevCarId = carId[i + 1].Id;
                    }

                    break;
                }
            }

            return true;
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