using _4_04LogInsNew.Models;
using Library.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _4_04LogInsNew.Controllers
{
    public class HomeController : Controller
    {
        Manager mgr = new Manager(Properties.Settings.Default.Constr);
        List<string> correct;
        ViewModel vm = new ViewModel();

        public ActionResult Index()
        {
            string message = "";
            if (TempData["Message"] != null)
            {
                message = (string)TempData["Message"];
            }
            return View();
        }
        [Authorize]
        public ActionResult Upload(int id)
        {           
            return View(id);
        }

    [HttpPost]
        public ActionResult Uploading(HttpPostedFileBase image, string password,int UserId)
        {
            string filename = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(image.FileName);
            string fullPath = $"{Server.MapPath("/My_Images")}\\{filename}";
            image.SaveAs(fullPath);
            int id = mgr.AddImage(filename, password,UserId);
            TempData["Message"] = $"Your file has been uploaded! You can share the following link: http://localhost:61739/Home/Image?image={id}";
            return Redirect("/account/loggedin");
        }

        public ActionResult ViewImage(int image, string password)
        {
            if (TempData["Message"] != null)
            {
                vm.Message = (string)TempData["Message"];
            }

            if (correct == null)
            {
                correct = new List<string>();
            }

            Images Current = mgr.GetImage(image);
            var fromSession = (List<string>)Session["CorrectPasswords"];
            if (password != null)
            {
                string pw = Current.Password;
                if (password != pw)
                {
                    TempData["Message"] = $"Password incorrect. Please try again.";
                    return Redirect($"/home/Image?image={image}");
                }
            }

            if (password != null || fromSession != null && fromSession.FirstOrDefault(c => c == Current.Password) != null)
            {
                vm.ViewImage = true;
            }
            Session["CorrectPasswords"] = correct;
            correct.Add(password);
            mgr.SetTimesViewed(image);
            vm.ImageCurrent = Current;
            vm.ImageCurrent.TimesViewed = mgr.GetTimes(image);
            return View(vm);

        }


    }
}
   