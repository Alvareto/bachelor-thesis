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

        // GET: Reservations
        [Authorize(Roles = CustomRoles.User + "," + CustomRoles.Administrator)]
        public ActionResult Index()
        {
            return View(db.ReservationSet.ToList());
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
            ViewBag.User = GetUser(reservation.Client_Id);
            if (ViewBag.User == null)
            {
                return HttpNotFound("Reservation user doesn't exist.");
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create(int? officeId)
        {
            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);
            ViewBag.Cars = db.CarSet.Where(car => car.Office.Id == officeId).ToList();

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

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateStarted,DateEnded")] Reservation reservation)
        {
            return new HttpStatusCodeResult(HttpStatusCode.NotImplemented, "Edit for the Reservation object is not supported. Please cancel the existing Reservation and then create a new one.");

            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
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
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.ReservationSet.Find(id);
            db.ReservationSet.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
                Car = db.CarSet.Find(carId)
            };
            //var o = (reservation.DateEnded - reservation.DateStarted).Days;
            db.ReservationSet.Add(reservation);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = reservation.Id });
        }

        public ActionResult Check([Bind(Include = "Id,DateStarted,DateEnded")] Reservation reservation, int officeId)
        {
            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);
            ViewBag.Cars = db.CarSet.Where(car => car.Office.Id == officeId).ToList();

            return View("Create", reservation);
        }

        /// <summary>
        /// Used to retrieve user profile information, like FirstName, LastName and City.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUser GetUser(String userId)
        {
            return UserManager.FindById(userId);
        }


    }
}
