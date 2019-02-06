using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupportManagemantSystem.Models;
namespace SupportManagemantSystem.Controllers
{
    public class MessageController : Controller
    {
        private SupportManagemantSystemEntities db = new SupportManagemantSystemEntities();
        // GET: Message
        User user = new User();
        public MessageController()
        {
            if (System.Web.HttpContext.Current.Session["RPG"] != null)
            {
                user = System.Web.HttpContext.Current.Session["RPG"] as User;
            }
        }
        [RequsetLogin(1, 2)]
        public ActionResult Index()
        {
            return View(db.Messages);
        }
        [RequsetLogin(1, 2)]

        public ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [RequsetLogin(1, 2)]

        public ActionResult Create(Message message, List<int> companyID)
        {
            foreach (var item in companyID)
            {
                message.MessageRecievers.Add(new MessageReciever() { companyID = item });
            }

            db.Messages.Add(message);

            db.SaveChanges();
            return View();
        }
        [RequsetLogin(1, 2, 3)]

        public ActionResult Details(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(404);
            }
            var model = db.Messages.Find(id);
            if (!(model.MessageRecievers.Any(c => c.Company.CompanyUsers.Any(w => w.userID == user.userID)) || user.userGroupID == 1 || user.userGroupID == 2))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            var users = db.Users.Select(c => c.CompanyUsers);
            var companies = new List<int>();
            foreach (var item in users)
            {
                foreach (var item2 in item)
                {
                    companies.Add(item2.Company.CompanyID);
                }
            }
            if (user.userGroupID == 3)
            {
                var sss = db.MessageRecievers.Where(c => c.messageID == model.messageID && companies.Contains(c.companyID.Value)).FirstOrDefault();
                sss.seen = true;
                db.SaveChanges();

            }
            ViewBag.GroupID = user.userGroupID;
            return View(model);
        }
        [RequsetLogin(3)]
        public ActionResult MyMessages()
        {
            var e = db.CompanyUsers.Where(c => c.userID == user.userID).Select(c => c.companyID);
            var model = db.Messages.Where(c => c.MessageRecievers.Any(w => e.Contains(w.Company.CompanyID)));
            return View(model);
        }
    }
}