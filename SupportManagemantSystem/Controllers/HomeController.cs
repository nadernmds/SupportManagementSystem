using SupportManagemantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupportManagemantSystem.Controllers
{
    [RequsetLogin]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = new User();
            if (Session["RPG"] != null)
            {
                user = Session["RPG"] as User;
            }
            if (user.userGroupID==3)
            {
                return RedirectToAction("CustomerIndex");
            }
            return View();
        }
        [RequsetLogin(3)]
        public ActionResult CustomerIndex()
        {
            return View();
        }
    }
}