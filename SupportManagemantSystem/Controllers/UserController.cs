using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SupportManagemantSystem.Models;
using System.Threading.Tasks;

namespace SupportManagemantSystem.Controllers
{

    public class UserController : Controller
    {
        private SupportManagemantSystemEntities db = new SupportManagemantSystemEntities();
        public ActionResult Login() { return View(); }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.Where(c => c.username == username && c.password == password).FirstOrDefault();
            if (user != null && user.userGroupID != 3)
            {
                Session["RPG"] = user;
                return RedirectToAction("index", "home");
            }
            else if (user != null && user.userGroupID == 3)
            {
                Session["RPG"] = user;
                return RedirectToAction("CustomerIndex", "home");
            }
            ViewBag.message = "نام کاربری یا کلمه عبور اشتباه است";
            return View();

        }
        public ActionResult LogOut()
        {
            Session["RPG"] = null;
            return View("Login");
        }
        [RequsetLogin(1, 2)]
        // GET: User
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserGroup);
            return View(users.ToList());
        }
        [RequsetLogin(1, 2)]
        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [RequsetLogin(1, 2)]

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.userGroupID = new SelectList(db.UserGroups, "userGroupID", "name");
            return View();
        }
        [RequsetLogin(1, 2)]

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,username,password,name,userGroupID,email")] User user, List<int> CompanyID, List<int> ProjectID)
        {
            if (ModelState.IsValid)
            {

                db.Users.Add(user);
                if (CompanyID != null)
                {
                    foreach (var item in CompanyID)
                    {
                        user.CompanyUsers.Add(new CompanyUser { companyID = item });
                    }
                }
                if (ProjectID != null)
                {
                    foreach (var item in ProjectID)
                    {
                        user.UserProjects.Add(new UserProject { projectID = item });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userGroupID = new SelectList(db.UserGroups, "userGroupID", "name", user.userGroupID);
            return View(user);
        }
        [RequsetLogin(1, 2)]

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.userGroupID = new SelectList(db.UserGroups, "userGroupID", "name", user.userGroupID);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequsetLogin(1, 2)]

        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,password,name,userGroupID,email")] User user, List<int> CompanyID, List<int> ProjectID)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                db.CompanyUsers.RemoveRange(db.CompanyUsers.Where(c => c.userID == user.userID));
                if (CompanyID != null)
                {
                    foreach (var item in CompanyID)
                    {
                        db.CompanyUsers.Add(new CompanyUser { companyID = item, userID = user.userID });
                    }
                }
                db.UserProjects.RemoveRange(db.UserProjects.Where(c => c.userID == user.userID));
                if (ProjectID != null)
                {
                    foreach (var item in ProjectID)
                    {
                        db.UserProjects.Add(new UserProject { projectID = item, userID = user.userID });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userGroupID = new SelectList(db.UserGroups, "userGroupID", "name", user.userGroupID);
            return View(user);
        }

        [RequsetLogin(1, 2)]
        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [RequsetLogin(1, 2)]

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
    }
}
