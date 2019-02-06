using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SupportManagemantSystem.Models;
namespace SupportManagemantSystem.Controllers
{
    [RequsetLogin(1,2)]
    public class DutyController : Controller
    {
        private SupportManagemantSystemEntities db = new SupportManagemantSystemEntities();
        User user = new User();
        public DutyController()
        {
            if (System.Web.HttpContext.Current.Session["RPG"] != null)
            {
                user = System.Web.HttpContext.Current.Session["RPG"] as User;
            }
        }
        // GET: Duty
        public ActionResult UserPanel()
        {
            return View(db.Duties.Where(c => c.referedToUserID == user.userID && c.done == false));
        }
        public ActionResult Index()
        {
            var duties = db.Duties.Include(d => d.Request).Include(d => d.User).Include(d => d.User1).Include(d => d.Piority).Include(d => d.Project);
            return View(duties.ToList());
        }
        public ActionResult start(long? id)
        {
            if (!db.Doings.Any(c => c.startdate.HasValue == true
            && c.endDate.HasValue == false))
            {
                db.Doings.Add(new Doing { dutyID = id, startdate = Common.GetDate(), userID = user.userID });
                db.SaveChanges();
                return Content("1");
            }

            return Content("0");
        }

        public ActionResult stop(long? id)
        {
            if (db.Doings.Any(c => c.startdate.HasValue == true
            && c.endDate.HasValue == false))
            {
                var u = db.Doings.Where(c => c.dutyID == id && c.endDate.HasValue == false).SingleOrDefault();
                u.endDate = Common.GetDate();
                u.timeDistanse = Convert.ToInt32(Math.Floor((u.endDate.Value.Subtract(u.startdate.Value)).TotalMinutes));
                db.SaveChanges();
                return Content("1");
            }

            return Content("0");
        }
        public ActionResult End(long? id)
        {
            if (db.Doings.Any(c => c.startdate.HasValue == true
            && c.endDate.HasValue 
            == false && c.dutyID == id))
            {
                return Content("0");
            }
            else{
                var u = db.Duties.Find(id);
                u.done = true;
                db.SaveChanges();
                return Content("1");
            }
        }
        // GET: Duty/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



        // GET: Duty/Create
        public ActionResult Create(long? id)
        {
            //ViewBag.requestID = new SelectList(db.Requests, "requestID", "text");
            //ViewBag.registerID = new SelectList(db.Users, "userID", "username");
            ViewBag.referedToUserID = new SelectList(db.Users.Where(c => c.UserProjects.Count > 0), "userID", "name");
            ViewBag.piorityID = new SelectList(db.Piorities, "piorityID", "piorityName");
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name");
            if (id.HasValue)
                return View(new Duty { requestID = id });
            return View();
        }

        // POST: Duty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dutyID,title,description,requiredTime,piorityID,done,requestID,registerID,referedToUserID,projectID")] Duty duty)
        {
            if (ModelState.IsValid)
            {
                duty.done = false;
                duty.registerID = user.userID;
                db.Duties.Add(duty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.referedToUserID = new SelectList(db.Users.Where(c => c.UserProjects.Count > 0), "userID", "name", duty.referedToUserID);
            ViewBag.piorityID = new SelectList(db.Piorities, "piorityID", "piorityName", duty.piorityID);
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name", duty.projectID);
            return View(duty);
        }


        // GET: Duties/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Duty duty = db.Duties.Find(id);
            if (duty == null)
            {
                return HttpNotFound();
            }
            ViewBag.referedToUserID = new SelectList(db.Users.Where(c => c.UserProjects.Count > 0), "userID", "name", duty.referedToUserID);
            ViewBag.piorityID = new SelectList(db.Piorities, "piorityID", "piorityName", duty.piorityID);
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name", duty.projectID);
            return View(duty);
        }

        // POST: Duties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "dutyID,title,description,requiredTime,piorityID,done,requestID,registerID,referedToUserID,projectID")] Duty duty)
        {
            if (ModelState.IsValid)
            {
                duty.registerID = user.userID;
                db.Entry(duty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.referedToUserID = new SelectList(db.Users.Where(c => c.UserProjects.Count > 0), "userID", "name", duty.referedToUserID);
            ViewBag.piorityID = new SelectList(db.Piorities, "piorityID", "piorityName", duty.piorityID);
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name", duty.projectID);
            return View(duty);
        }

        // GET: Duties/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Duty duty = db.Duties.Find(id);
            if (duty == null)
            {
                return HttpNotFound();
            }
            return View(duty);
        }

        // POST: Duties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Duty duty = db.Duties.Find(id);
            db.Duties.Remove(duty);
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
