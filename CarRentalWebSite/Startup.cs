using System;
using CarRental.EntityFramework;
using CarRentalWebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarRentalWebSite.Startup))]

namespace CarRentalWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }


        // In this method we will create default User roles and Admin user for login   
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup crete Admin Role and a default Admin User    
            if (!roleManager.RoleExists(CustomRoles.Administrator))
            {

                // first we create Admin role
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = CustomRoles.Administrator };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@fer.hr"
                };

                string userPWD = "admin";

                var chkUser = userManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, CustomRoles.Administrator);

                }
            }

            // creating Creating Client role    
            if (!roleManager.RoleExists(CustomRoles.User))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Name = CustomRoles.User };
                roleManager.Create(role);

            }

        }
    }
}