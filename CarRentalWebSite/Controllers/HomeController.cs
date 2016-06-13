using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.EntityFramework;
using CarRentalWebSite.Models;
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

            return View(new CarDetailsViewModel(db.CarSet.FirstOrDefault()));
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