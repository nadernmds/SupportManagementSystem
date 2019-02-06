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
    [RequsetLogin(1, 2)]
    public class TaskController : Controller
    {
        private SupportManagemantSystemEntities db = new SupportManagemantSystemEntities();
        User user = new User();
        public TaskController()
        {
            if (System.Web.HttpContext.Current.Session["RPG"] != null)
            {
                user = System.Web.HttpContext.Current.Session["RPG"] as User;
            }
        }
        
        // GET: Task
        public ActionResult Index()
        {
            var tasks = db.Tasks.Where(c => c.userID == user.userID).Include(t => t.Company).Include(t => t.Project).Include(t => t.Ticket);
            return View(tasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }
        [HttpPost]
        public ActionResult TicketInformation(long id)
        {
            var w = db.Tickets.Where(c => c.ticketID == id).Select(c => new { c = c.companyID, p = c.projectID });
            return Json(w, JsonRequestBehavior.AllowGet);
        }
        // GET: Task/Create
        public ActionResult Create()
        {
            ViewBag.companyID = new SelectList(db.Companies, "CompanyID", "name");
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name");
            ViewBag.ticketID = new SelectList(db.Tickets.Where(c => c.statusID != 4 && c.statusID != 1), "ticketID", "title");
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "taskID,decription,date,spentTime,ticketID,projectID,companyID")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.userID = user.userID;
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.companyID = new SelectList(db.Companies, "CompanyID", "name", task.companyID);
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name", task.projectID);
            ViewBag.ticketID = new SelectList(db.Tickets.Where(c => c.statusID != 4 && c.statusID != 1), "ticketID", "title", task.ticketID);
            return View(task);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.companyID = new SelectList(db.Companies, "CompanyID", "name", task.companyID);
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name", task.projectID);
            ViewBag.ticketID = new SelectList(db.Tickets, "ticketID", "title", task.ticketID);
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "taskID,decription,date,spentTime,ticketID,projectID,companyID")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.userID = user.userID;
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.companyID = new SelectList(db.Companies, "CompanyID", "name", task.companyID);
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name", task.projectID);
            ViewBag.ticketID = new SelectList(db.Tickets, "ticketID", "title", task.ticketID);
            return View(task);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
