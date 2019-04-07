using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Library.Data;

namespace _4_04LogInsNew.Controllers
{
    public class AccountController : Controller
    {
        UserManagers mgr = new UserManagers(Properties.Settings.Default.Constr);
        Manager mngr = new Manager(Properties.Settings.Default.Constr);
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogIn()
        {
            return View();
                    }
        public ActionResult LoggingIn(string email,string password )
        {
           User user = mgr.Login(email,password );
            if (user == null)
            {
                return Redirect("/account/LogIn");
            }

            FormsAuthentication.SetAuthCookie(user.Email, true);
            return Redirect($"/account/LoggedIn?name={user.Name}&id={user.Id}");
          
        }

        public ActionResult LoggedIn(string name , int id)
        {
            User u = new User
            {
                Id = id,
                Name = name
            };
            return View(u);
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult SigningUp(string password, User user)
        {
            mgr.AddUser(user, password);
            return Redirect("/home/index");
        }

        //[Authorize]
        public ActionResult MyImages(int id)
        {          
            return View(mngr.MyImages(id));
        }
    }

}