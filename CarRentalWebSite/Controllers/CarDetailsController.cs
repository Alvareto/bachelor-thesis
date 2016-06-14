using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CarRental.EntityFramework;
using CarRentalWebSite.Models;

namespace CarRentalWebSite
{
    public class CarDetailsController : Controller
    {
        private CarRentalWebSiteContext db = new CarRentalWebSiteContext();

        // GET: CarDetails/ManageAll
        public ActionResult ManageAll(int? carId)
        {
            CarSpecificationViewModel cs;
            if (carId.HasValue)
            {
                ViewBag.carId = carId;
                var car = db.CarSet.Find(carId.Value);
                cs = new CarSpecificationViewModel(car);
            }
            else
                cs = new CarSpecificationViewModel();

            return View(cs);
        }

        // POST: CarDetails/ManageAll
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageAll(CarSpecificationViewModel model, int carId)
        {
            Car car = db.CarSet.Find(carId);
            ICollection<CarDetail> carSpecification = db.CarSet.Find(carId).CarDetails;
            CarDetail carDetail;

            if (carSpecification.Any(o => o.Type == CarDetailType.GasTankCapacity))
            {
                carDetail = carSpecification.Single(detail => detail.Type == CarDetailType.GasTankCapacity);
                carDetail.Value = model.GasTankCapacity.Value;
                db.Entry(carDetail).State = EntityState.Modified;

            }
            else
            {
                model.GasTankCapacity.Car = car;
                db.CarDetailSet.Add(model.GasTankCapacity);
            }


            if (carSpecification.Any(o => o.Type == CarDetailType.EnginePower))
            {
                carDetail = carSpecification.Single(detail => detail.Type == CarDetailType.EnginePower);
                carDetail.Value = model.EnginePower.Value;
                db.Entry(carDetail).State = EntityState.Modified;

            }
            else
            {
                model.EnginePower.Car = car;
                db.CarDetailSet.Add(model.EnginePower);
            }

            if (carSpecification.Any(o => o.Type == CarDetailType.FuelConsumption))
            {
                carDetail = carSpecification.Single(detail => detail.Type == CarDetailType.FuelConsumption);
                carDetail.Value = model.FuelConsumption.Value;
                db.Entry(carDetail).State = EntityState.Modified;

            }
            else
            {
                model.FuelConsumption.Car = car;
                db.CarDetailSet.Add(model.FuelConsumption);
            }

            db.SaveChanges();

            return RedirectToAction("Details", "Cars", new { id = carId });
        }

        // GET: CarDetails
        //public ActionResult Index()
        //{
        //    return View(db.CarDetailSet.ToList());
        //}

        // GET: CarDetails/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CarDetail carDetail = db.CarDetailSet.Find(id);
        //    if (carDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carDetail);
        //}

        //// GET: CarDetails/Create
        //public ActionResult Create(int? carId)
        //{
        //    var carDetail = new CarDetail();
        //    if (carId.HasValue)
        //    {
        //        var car = db.CarSet.Find(carId.Value);
        //        carDetail.Car = car;
        //    }
        //    return View(carDetail);
        //}

        //// POST: CarDetails/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Type,Value")] CarDetail carDetail, int carId)
        //{
        //    carDetail.Car = db.CarSet.Find(carId);
        //    if (ModelState.IsValid)
        //    {
        //        db.CarDetailSet.Add(carDetail);
        //        db.SaveChanges();
        //        return RedirectToAction("Details", "Cars", new { id = carId });
        //    }

        //    return View(carDetail);
        //}


        // GET: CarDetails/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CarDetail carDetail = db.CarDetailSet.Find(id);
        //    if (carDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carDetail);
        //}

        //// POST: CarDetails/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Type,Value")] CarDetail carDetail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(carDetail).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(carDetail);
        //}

        //// GET: CarDetails/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CarDetail carDetail = db.CarDetailSet.Find(id);
        //    if (carDetail == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carDetail);
        //}

        //// POST: CarDetails/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    CarDetail carDetail = db.CarDetailSet.Find(id);
        //    db.CarDetailSet.Remove(carDetail);
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
    }
}
