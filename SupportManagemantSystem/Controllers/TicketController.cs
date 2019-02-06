using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupportManagemantSystem.Models;
using System.Data.Entity;
using System.IO;

namespace SupportManagemantSystem.Controllers
{
    [RequsetLogin]

    public class TicketController : Controller
    {
        Models.SupportManagemantSystemEntities db = new Models.SupportManagemantSystemEntities();
        User user = new User();
        public TicketController()
        {
            if (System.Web.HttpContext.Current.Session["RPG"] != null)
            {
                user = System.Web.HttpContext.Current.Session["RPG"] as User;
            }

        }
        // GET: Ticket
        [RequsetLogin(1, 2)]
        public ActionResult Index()
        {
            return View(db.Tickets);
        }
        public ActionResult AskAgain(long? id)
        {
            var model = db.Tickets.Find(id);

            return RedirectToAction("ask", new { id = model.companyID, tid = id });
        }
        [RequsetLogin(3)]
        public ActionResult MyTickets() => View(db.Tickets.Where(c => c.Requests.Any(w => w.askerID == user.userID)));
        [RequsetLogin]
        public ActionResult Files(long? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(404);
            }
            return View(db.TicketFiles.Where(c => c.requestID == id || c.responseID == id));
        }
        public ActionResult Answer(long id)
        {
            ViewBag.statusID = new SelectList(db.Status, "statusID", "state", 2);
            var model = db.Tickets.Find(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequsetLogin(1, 2)]
        public ActionResult Answer(Response response, byte statusID, long requestID, HttpPostedFileBase[] file)
        {
            if (ModelState.IsValid)
            {
                var ticket = db.Tickets.Find(response.ticketID);
                response.answererID = user.userID;
                ticket.statusID = statusID;
                response.date = Common.GetDate();
                ticket.Responses.Add(response);
                db.Entry(ticket).State = EntityState.Modified;

                //ticket.answererID = user.userID;
                //ticket.answer = ticket.answer;
                //ticket.Ticket.statusID = statusID;
                //db.Responses.Add(ticket);
                db.SaveChanges();
                if (file[0] != null)
                {
                    foreach (var item in file)
                    {
                        var filename = Path.GetExtension(item.FileName);
                        var ticketFile = new TicketFile() { fileName = filename, responseID = response.responseID };
                        db.TicketFiles.Add(ticketFile);
                        db.SaveChanges();
                        item.SaveAs(Server.MapPath("~/files/" + ticketFile.ticketFileID + filename));
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.statusID = new SelectList(db.Status, "statusID", "state");
            return View(response);
        }
        [RequsetLogin(3)]
        public ActionResult Ask(int? id, long? tid)
        {
            TempData["CompanyID"] = id;
            TempData["ticketID"] = tid;
            ViewBag.projectID = new SelectList(db.CompanyProjects.Where(c => c.companyID == id).Select(c => c.Project), "projectID", "name");

            if (tid.HasValue)
            {
                return View(db.Tickets.Find(tid));
            }
            return View(new Ticket());
        }
        [RequsetLogin(3)]
        public ActionResult SelectCompany() => View(db.CompanyUsers.Where(c => c.userID == user.userID).Select(c => c.Company));
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequsetLogin(3)]
        public ActionResult Ask(Ticket ticket, string text, HttpPostedFileBase[] file, int? projectID)
        {
            if (ModelState.IsValid)
            {
                var mode = ticket.ticketID != 0;
                if (mode)
                {
                    ticket = db.Tickets.Find(ticket.ticketID);
                }
                int? comanyID = (int?)TempData["CompanyID"] ?? 0;
                if (comanyID == 0)
                    return RedirectToAction("SelectCompany");
                var request = new Request { text = text, askerID = user.userID,date= Common.GetDate() };
                ticket.Requests.Add(request);
                ticket.companyID = comanyID;
                ticket.projectID = projectID;
                ticket.statusID = 1;
                if (mode)
                {
                    db.Entry(ticket).State = EntityState.Modified;
                }
                else
                {
                db.Tickets.Add(ticket);

                }
                db.SaveChanges();
                if (file[0] != null)
                {
                    foreach (var item in file)
                    {
                        var filename = Path.GetExtension(item.FileName);
                        var ticketFile = new TicketFile() { fileName = filename, requestID = request.requestID };
                        db.TicketFiles.Add(ticketFile);
                        db.SaveChanges();
                        item.SaveAs(Server.MapPath("~/files/" + ticketFile.ticketFileID + filename));
                    }
                }
                db.SaveChanges();

                return RedirectToAction("CustomerIndex", "home", new ViewDataDictionary { new KeyValuePair<string, object>("message", "تیکت شما با موفقیت ارسال شد لطفا تا دریافت نتیجه صبور باشید") });
            }
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name", ticket.projectID);
            return View(ticket);
        }
        // GET: Ticket/Details/5
        [RequsetLogin(1, 2)]
        public ActionResult Details(int id)
        {
            return View();
        }
        [RequsetLogin(1, 2)]
        // GET: Ticket/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(1, 2)]
        // POST: Ticket/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [RequsetLogin(1, 2)]
        // GET: Ticket/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        [RequsetLogin(1, 2)]
        // POST: Ticket/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [RequsetLogin(1, 2)]
        // GET: Ticket/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        [RequsetLogin(1, 2)]
        // POST: Ticket/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
