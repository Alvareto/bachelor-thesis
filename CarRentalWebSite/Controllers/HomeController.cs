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
                officeId = car.Office_Id;
            }
            else
            {
                // carId == null && officeId == null  => first office -> first car
                if (officeId == null)
                {
                    var officeFirst = db.OfficeSet.FirstOrDefault();
                    if (officeFirst != null)
                        officeId = officeFirst.Id;
                }

                // carId == null && officeId != null => first car in office
                if (db.CarSet.Any(o => o.Office.Id == officeId))
                {
                    car = db.CarSet.First(o => o.Office.Id == officeId);
                }
                else
                {// show message - "no cars found"
                    ModelState.AddModelError("", "Trenutno nije dostupan niti jedan automobil u odabranom gradu.");
                    ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);
                    TempData["Found"] = false;
                    return View(new CarDetailsViewModel());
                }
            }

            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);

            // previousCar, nextCar

            SetNextAndPrevCars(car.Id);

            var model = new CarDetailsViewModel(car, new Reservation())
            {
                NextCar = nextCar,
                PrevCar = prevCar,
                IsAvailable = ViewBag.Available = false
            };

            //ViewBag.Available = false;
            TempData["Found"] = true;

            return View(model);
        }


        public ActionResult Check([Bind(Include = "Id,DateStarted,DateEnded")] Reservation reservation, int? officeId, int? carId)
        {
            if (officeId == null || reservation == null || carId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (reservation.DateStarted.Date > reservation.DateEnded.Date)
            {
                ModelState.AddModelError("", "Datum početka rezervacije mora biti prije datuma kraja rezervacije");
            }
            if (reservation.DateStarted.Date < DateTime.Today.Date)
            {
                ModelState.AddModelError("", "Datum početka rezervacije mora biti u budućnosti");
            }
            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);

            Car car = db.CarSet.Find(carId);
            SetNextAndPrevCars(carId.Value);
            var model = new CarDetailsViewModel(car, reservation)
            {
                NextCar = nextCar,
                PrevCar = prevCar,
                IsAvailable = Available(reservation, car)
            };
            ViewBag.Available = model.IsAvailable; // = Available(model.Reservation, model.Car);
            //if (!ModelState.IsValid) ViewBag.Available = false;

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


        private Car prevCar;


        private Car nextCar;

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

            var carId = db.CarSet.Where(n => n.Office_Id == car.Office_Id)
                                  .OrderBy(n => n.Id).ToArray();

            for (int i = 0, N = carId.Length; i < N; i++)
            {
                if (carId[i].Id == id)
                {
                    if (i > 0)
                    {
                        nextCar = carId[i - 1];
                    }

                    if (i < N - 1)
                    {
                        prevCar = carId[i + 1];
                    }

                    break;
                }
            }

            return true;
        }


        /// <summary>
        /// Used to retrieve user profile information, like FirstName, LastName and City.
        /// </summary>
        /// <param name="userId">Reservation.Client_Id</param>
        /// <returns>ApplicationUser</returns>
        public ApplicationUser GetUser(String userId)
        {
            return UserManager.FindById(userId);
        }

        public String UserName(String userId)
        {
            var u = GetUser(userId);
            if (string.IsNullOrEmpty(u.FirstName) && string.IsNullOrEmpty(u.LastName))
                return u.UserName;
            return u.FirstName + " " + u.LastName;
        }
    }
}