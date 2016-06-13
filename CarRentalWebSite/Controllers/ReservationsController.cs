using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRental.EntityFramework;
using CarRentalWebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRentalWebSite
{
    public class ReservationsController : Controller
    {
        private CarRentalWebSiteContext db = new CarRentalWebSiteContext();
        private ApplicationUserManager _userManager;

        public ReservationsController()
        {
        }

        public ReservationsController(ApplicationUserManager userManager)
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

        /// <summary>
        /// This action is used to display all Reservations if User has Administrator role and just personal Reservations if not.
        /// </summary>
        /// <returns></returns>
        // GET: Reservations
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Administrator)]
        public ActionResult Index()
        {
            var reservations = db.ReservationSet.Where(reservation => !reservation.Canceled).ToList();
            if (User.IsInRole(CustomRoles.User))
            {
                reservations = reservations.Where(reservation => reservation.Client_Id == User.Identity.GetUserId()).ToList();
            }
            return View(reservations);
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.ReservationSet.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            var u = GetUser(reservation.Client_Id);
            if (u == null)
            {
                return HttpNotFound("Reservation user doesn't exist.");
            }
            ViewBag.UserName = u.FirstName + " " + u.LastName;

            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create(int? officeId)
        {
            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);
            ViewBag.Cars = new List<Car>(); //db.CarSet.Where(car => car.Office.Id == officeId).ToList();

            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateStarted,DateEnded")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.Canceled = false;
                db.ReservationSet.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotImplemented, "Edit for the Reservation object is not supported. Please cancel the existing Reservation and then create a new one.");
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotImplemented, "Delete for the Reservation object is not supported. Please cancel the existing Reservation instead.");

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Reservation reservation = db.ReservationSet.Find(id);
            //if (reservation == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(reservation);
        }

        // POST: Reservations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Reservation reservation = db.ReservationSet.Find(id);
        //    db.ReservationSet.Remove(reservation);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

            return RedirectToAction("Details", new { id = reservation.Id });
        }

        public ActionResult Check([Bind(Include = "Id,DateStarted,DateEnded")] Reservation reservation, int? officeId)
        {
            if (officeId == null)
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
            ViewBag.Cars = //db.CarSet.Where(car => car.Office.Id == officeId).ToList();

            FilterCars(reservation, db.CarSet.Where(car => car.Office.Id == officeId).ToList()).ToList();

            return View("Create", reservation);
        }

        private HashSet<Car> FilterCars(Reservation reservation, List<Car> cars)
        {
            // cars -> reservation dates overlap ? -> return if not
            HashSet<Car> set = new HashSet<Car>();
            //List<Car> list = new List<Car>();
            foreach (Car car in cars)
                foreach (var r in car.Reservations.Where(res => !res.Canceled))
                {
                    if (!DateRangesOverlap(reservation.DateStarted, reservation.DateEnded, r.DateStarted, r.DateEnded))
                        set.Add(car);
                }
            return set;
        }

        private bool DateRangesOverlap(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
        {
            //bool overlap = a.start < b.end && b.start < a.end;
            return aStart.Date <= bEnd.Date && bStart.Date <= aEnd.Date;
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

        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.ReservationSet.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }

            reservation.Canceled = true;

            db.Entry(reservation).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
