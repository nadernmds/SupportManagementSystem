using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SupportManagemantSystem.Models;
using pep;
namespace SupportManagemantSystem.Controllers
{
    [RequsetLogin(1,2)]
    public class ReportController : Controller
    {
        SupportManagemantSystemEntities db = new SupportManagemantSystemEntities();
        // GET: Report
        public ActionResult Index()
        {

            ViewBag.userID = new SelectList(db.Users, "userID", "name");
            ViewBag.projectID = new SelectList(db.Projects, "projectID", "name");
            ViewBag.companyID = new SelectList(db.Companies, "companyID", "name");
            return View();
        }
        [HttpPost]
        public ActionResult generate(long? userID, int? projectID, int? companyID, string from, string to, string state)
        {
            DateTime fromdate = DateTime.MinValue;
            DateTime todate = DateTime.MaxValue;
            if (!string.IsNullOrEmpty(from))
                fromdate = from.toMiladiDate();
            if (!string.IsNullOrEmpty(to))
                todate = to.toMiladiDate().AddDays(1);
            var model = db.Duties.Where(c =>
            (userID.HasValue ? c.Doings.Any(w => w.userID == userID) : true)
            && (projectID.HasValue ? c.projectID == projectID.Value : true)
            && (companyID.HasValue ? c.companyID == companyID.Value : true)
            && c.Doings.Any(w => w.startdate >= fromdate)
            && c.Doings.Any(w => w.startdate <= todate)
            && c.Doings.Sum(w => w.timeDistanse) > 0
            )
            .Select(c => new WorkSheet
            {
                CompanyName = c.Company.name,
                Company = c.Company,
                Project = c.Project,
                projectName = c.Project.name,
                User = c.Doings.Where(w => w.startdate >= fromdate && w.startdate <= todate && w.timeDistanse > 0)
                   .Select(w => new WorkSheetUser
                   {
                       user = w.User.name,
                       Minutes = w.timeDistanse.Value,
                       Date = w.startdate.Value
                   })
            });
            switch (state)
            {
                case "full":
                    return PartialView("FullReport", model);

                case "detailed":
                    return PartialView("DetailedReport", model);
                case "CompanyAndProject":
                    var CompanyAndProject = (from u in db.Duties
                                             where (userID.HasValue ? u.Doings.Any(w => w.userID == userID) : true)
                          && (projectID.HasValue ? u.projectID == projectID.Value : true)
                          && (companyID.HasValue ? u.companyID == companyID.Value : true)
                          && u.Doings.Any(w => w.startdate >= fromdate)
                          && u.Doings.Any(w => w.startdate <= todate)
                                             select new
                                             {
                                                 company = u.Company.name,
                                                 project = u.Project.name,
                                                 hours = u.Doings.Sum(c => c.timeDistanse)
                                             }).GroupBy(c => c.company)
                               .Select(c => new CompanyProjectWorkSheet
                               {
                                   CompanyName = c.Key,
                                   hours = c.Sum(w => w.hours).Value,
                                   ProjectNames = c.GroupBy(w => w.project).Select(w =>
                                                  new ProjectHours { ProjectName = w.Key, hours = w.Sum(f => f.hours) })
                               }).ToList();
                    return PartialView("CompanyAndProject", CompanyAndProject);
                case "ProjectAndCompany":
                    var ProjectAndCompany = (from u in db.Duties
                                             where (userID.HasValue ? u.Doings.Any(w => w.userID == userID) : true)
                          && (projectID.HasValue ? u.projectID == projectID.Value : true)
                          && (companyID.HasValue ? u.companyID == companyID.Value : true)
                          && u.Doings.Any(w => w.startdate >= fromdate)
                          && u.Doings.Any(w => w.startdate <= todate)
                                             select new
                                             {
                                                 company = u.Company.name,
                                                 project = u.Project.name,
                                                 hours = u.Doings.Sum(c => c.timeDistanse)
                                             }).Where(c => c.hours > 0).GroupBy(c => c.project)
                               .Select(c => new ProjectCompanyWorkSheet
                               {
                                   ProjectName = c.Key,
                                   hours = c.Sum(w => w.hours).Value,
                                   CompanyNames = c.GroupBy(w => w.company).Select(w =>
                                                  new CompanyHours { CompanyName = w.Key, hours = w.Sum(f => f.hours) })
                               }).ToList();
                    return PartialView("ProjectAndCompany", ProjectAndCompany);
                default:
                    break;
            }
            return Content("نادرست");
        }
    }


}