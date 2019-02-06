using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using pep;
namespace SupportManagemantSystem.Models
{
    public class WorkSheet
    {
        [Key]
        public int Id { get; set; }
        public int? Minutes
        {
            get
            {
                return User.Sum(c => c.Minutes);
            }

            private set { }
        }
        public Company Company { get; set; }
        public Project Project { get; set; }
        public string projectName { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<WorkSheetUser> User { get; set; }
        public string ticket { get; set; }
        //public IEnumerable<DateTime> Mdate { get; set; }
        //public IEnumerable<string> Pdate => Mdate.ToPersianDate().Distinct();
    }
    public class WorkSheetUser
    {
        public int Minutes { get; set; }
        public string user { get; set; }
        public string PersianDate => Date.ToPersianDateShortString();
        public DateTime Date { get; set; }
    }
    public class CompanyProjectWorkSheet
    {
        public string CompanyName { get; set; }
        public IEnumerable<ProjectHours> ProjectNames { get; set; }
        public int? hours { get; set; }
    }
    public class ProjectHours
    {
        public string ProjectName { get; set; }
        public int? hours { get; set; }
    }

    public class ProjectCompanyWorkSheet
    {
        public string ProjectName { get; set; }
        public IEnumerable<CompanyHours> CompanyNames { get; set; }
        public int? hours { get; set; }
    }
    public class CompanyHours
    {
        public string CompanyName { get; set; }
        public int? hours { get; set; }
    }
}