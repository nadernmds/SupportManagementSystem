 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SupportManagemantSystem.Models;
using pep;
namespace SupportManagemantSystem.Controllers
{
    public class EXEFileController : Controller
    {
        private SupportManagemantSystemEntities db = new SupportManagemantSystemEntities();
        // GET: EXE
        [HttpPost]
        public ActionResult CheckForUpdates(string id /*version*/)
        {
            var exeID = db.EXEFiles.SingleOrDefault(c => c.version == id).exeID;
            return Json(db.EXEFiles.Any(c => c.exeID > exeID));
        }

        public ActionResult Download(string version, bool latest)
        {
            version = version.Replace('_', '.');
            var exeID = db.EXEFiles.SingleOrDefault(c => c.version == version).exeID;
            if (db.EXEFiles.Any(c => c.exeID > exeID))
            {

                var max = db.EXEFiles.Where(c => c.exeID == db.EXEFiles.Max(w => w.exeID)).FirstOrDefault();
                return File(Server.MapPath("~/files/exe/" + max.exeID + ".rar"), "exe", "HepsaAccountingSystem.rar");

            }
            return File(new byte[0], "exe");
        }
        [RequsetLogin(1, 2)]
        // GET: EXE
        public ActionResult Index()
        {
            return View(db.EXEFiles.ToList());
        }
        [RequsetLogin(1, 2)]

        // GET: EXE/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXEFile eXE = db.EXEFiles.Find(id);
            if (eXE == null)
            {
                return HttpNotFound();
            }
            return View(eXE);
        }
        [RequsetLogin(1, 2)]

        // GET: EXE/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EXE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequsetLogin(1, 2)]

        [ValidateAntiForgeryToken]
        public ActionResult Create(EXEFile ddd, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                ddd.date = Common.GetDate();
                db.EXEFiles.Add(ddd);
                db.SaveChanges();
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/files/exe/") + ddd.exeID + ".rar");
                }
                return RedirectToAction("Index");
            }

            return View(ddd);
        }
        [RequsetLogin(1, 2)]

        // GET: EXE/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXEFile eXE = db.EXEFiles.Find(id);
            if (eXE == null)
            {
                return HttpNotFound();
            }
            return View(eXE);
        }

        // POST: EXE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequsetLogin(1, 2)]

        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "exeID,version,changes,date")] EXEFile ddd, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ddd).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/files/exe/") + ddd.exeID + ".rar");
                }
                return RedirectToAction("Index");
            }
            return View(ddd);
        }
        [RequsetLogin(1, 2)]

        // GET: EXE/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EXEFile eXE = db.EXEFiles.Find(id);
            if (eXE == null)
            {
                return HttpNotFound();
            }
            return View(eXE);
        }
        [RequsetLogin(1, 2)]

        // POST: EXE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            EXEFile eXE = db.EXEFiles.Find(id);
            db.EXEFiles.Remove(eXE);
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
