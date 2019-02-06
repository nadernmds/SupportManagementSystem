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
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace SupportManagemantSystem.Controllers
{
    public class QueryController : Controller
    {
        private SupportManagemantSystemEntities db = new SupportManagemantSystemEntities();
        User user = new User();
        public QueryController()
        {
            if (System.Web.HttpContext.Current.Session["RPG"] != null)
            {
                user = System.Web.HttpContext.Current.Session["RPG"] as User;
            }
        }
        [RequsetLogin(1, 2)]

        // GET: Query
        public ActionResult Index()
        {
            return View(db.Queries);
        }

        public ActionResult ErrorReports()
        {
            return View(db.Errors);
        }
        [RequsetLogin(1, 2)]

        public ActionResult ErrorFixed(long done)
        {
            var s = db.Errors.Find(done);
            s.done = true;
            db.SaveChanges();
            return RedirectToAction("ErrorReports");
        }
        [RequsetLogin(1, 2)]

        // GET: Query/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Query query = db.Queries.Find(id);
            if (query == null)
            {
                return HttpNotFound();
            }
            return View(query);
        }
        [RequsetLogin(1, 2)]

        // GET: Query/Create
        public ActionResult Create()
        {
            ViewBag.type = new SelectList(db.QueryTypes, "queryTypeID", "name");
            return View();
        }
        public async Task<ActionResult> GetQueries(int? companyID, int? projectID, string FromDate = "1380/01/01", string ToDate = "1450/01/01", long FromNumber = 0, long ToNumber = long.MaxValue)
        {
            DateTime? FromMiladi = /*db.GetLastExecute(companyID, projectID).FirstOrDefault() ??*/ FromDate.toMiladiDate();
            DateTime? ToMiladi = ToDate.toMiladiDate();
            var queries = await db.Queries.Where(c => c.date >= FromMiladi && c.date <= ToMiladi && c.queryID >= FromNumber && c.queryID <= ToNumber).Select(c => new { id = c.queryID, command = c.queryCommand, companyID = companyID.Value, projectID = projectID.Value }).ToListAsync();
            return Json(queries, JsonRequestBehavior.AllowGet);
            //StringBuilder sb = new StringBuilder();
            //var flag = string.IsNullOrEmpty(DataBaseName);
            //foreach (var item in queries)
            //{
            //    if (!flag)
            //    {
            //        sb.AppendLine("use " + DataBaseName);
            //    }
            //    sb.AppendLine("go");
            //    sb.AppendLine(item.queryCommand);
            //    sb.AppendLine(" go ");
            //}
            //return Content(sb.ToString(), "text/plain");
        }
        // POST: Query/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequsetLogin(1, 2)]

        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "queryID,queryCommand,date,userID,type")] Query query)
        {
            if (ModelState.IsValid)
            {
                db.Queries.Add(query);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type = new SelectList(db.QueryTypes, "queryTypeID", "name");
            return View(query);
        }
        [RequsetLogin(1, 2)]

        // GET: Query/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Query query = db.Queries.Find(id);
            if (query == null)
            {
                return HttpNotFound();
            }
            ViewBag.type = new SelectList(db.QueryTypes, "queryTypeID", "name", query.type);
            return View(query);
        }

        // POST: Query/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequsetLogin(1, 2)]

        public ActionResult Edit([Bind(Include = "queryID,queryCommand,date,userID,type")] Query query)
        {
            if (ModelState.IsValid)
            {
                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type = new SelectList(db.QueryTypes, "queryTypeID", "name", query.type);
            return View(query);
        }
        [RequsetLogin(1, 2)]

        // GET: Query/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Query query = db.Queries.Find(id);
            if (query == null)
            {
                return HttpNotFound();
            }
            return View(query);
        }

        // POST: Query/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RequsetLogin(1, 2)]

        public ActionResult DeleteConfirmed(long id)
        {
            Query query = db.Queries.Find(id);
            db.Queries.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Error(HttpPostedFileBase file)
        {
            var s = Request.Files;
            BinaryReader b = new BinaryReader(file.InputStream);
            byte[] binData = b.ReadBytes(file.ContentLength);
            string result = Encoding.UTF8.GetString(binData);
            var jsonErrorModels = JsonConvert.DeserializeObject<IEnumerable<JsonErrorModel>>(result);
            foreach (var jsonErrorModel in jsonErrorModels)
            {
                db.Errors.Add(new Error { errorCodes = result, done = false, companyID = jsonErrorModel.companyID, projectID = jsonErrorModel.projectID, });
                foreach (var item in jsonErrorModel.Done)
                {
                    db.Executes.Add(new Execute { queryID = item, date = jsonErrorModel.date, CompanyID = jsonErrorModel.companyID, projectID = jsonErrorModel.projectID });
                }
            }
            db.SaveChanges();
            ViewBag.message = "با موفقیت ارسال شد";
            return View();
        }
        [RequsetLogin(3)]
        public ActionResult SelectCompany() => View(db.CompanyUsers.Where(c => c.userID == user.userID).Select(c => c.Company).Select(c => new SelectCompanyModel { projects = c.CompanyProjects, companyID = c.CompanyID, name = c.name }));

        [HttpPost]
        public ActionResult Settings(int? companyID, int? projectID)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"Windows Registry Editor Version 5.00");
            sb.AppendLine(@"[HKEY_CURRENT_USER\Software\Rizkaran\Hepsa]");
            sb.AppendLine("\"CompanyID\"=" + "\"" + companyID + "\"");
            sb.AppendLine("\"ProjectID\"" + "=" + "\"" + projectID + "\"");
            var w = sb.ToString();
            return File(Encoding.UTF8.GetBytes(w), "text/plain", "setting.reg");
        }

        //public ActionResult Download()
        //{
        //    var filesCol = obj.GetFile().ToList();
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //        {
        //            for (int i = 0; i < filesCol.Count; i++)
        //            {
        //                ziparchive.CreateEntryFromFile(filesCol[i].FilePath, filesCol[i].FileName);
        //            }
        //        }
        //        return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
        //    }
        //}
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
