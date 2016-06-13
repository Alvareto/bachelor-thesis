using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRental.EntityFramework;
using CarRentalWebSite.Models;

namespace CarRentalWebSite
{
    public class CarsController : Controller
    {
        private CarRentalWebSiteContext db = new CarRentalWebSiteContext();

        // GET: Cars
        public ActionResult Index()
        {
            return View(db.CarSet.ToList());
        }

        // GET: Cars/Search
        public ActionResult Search(int? officeId)
        {
            var s = new SelectList(db.OfficeSet, "Id", "City", officeId);
            s.ToList().Add((new SelectListItem() { Text = "", Value = "", Selected = true }));
            ViewBag.Offices = s;
            var cars = db.CarSet.ToList();
            if (officeId.HasValue)
            {
                ViewBag.OfficeId = officeId.Value;
                var office = db.OfficeSet.Find(officeId.Value);
                cars = cars.Where(o => o.Office == office).ToList();
            }
            return View(cars);
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var car = db.CarSet.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            //car.GetOffSeasonPriceWithDiscount();
            return View(car);
        }


        // GET: Cars/Create
        public ActionResult Create(int? officeId)
        {

            ViewBag.Offices = new SelectList(db.OfficeSet, "Id", "City", officeId);

            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Manufacturer,Model,Price")] Car car, int officeId)
        {
            if (ModelState.IsValid)
            {
                car.Office = db.OfficeSet.Find(officeId);
                db.CarSet.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.CarSet.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Manufacturer,Model,Price,Office_Id")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.CarSet.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.CarSet.Find(id);
            db.CarSet.Remove(car);
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

        private bool DateRangesOverlap(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
        {
            //bool overlap = a.start < b.end && b.start < a.end;
            return aStart.Date <= bEnd.Date && bStart.Date <= aEnd.Date;
        }

    }
}
