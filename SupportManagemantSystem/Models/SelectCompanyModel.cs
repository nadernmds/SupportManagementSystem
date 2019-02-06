using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportManagemantSystem.Models
{
    public class SelectCompanyModel
    {
        public int companyID { get; set; }
        public IEnumerable<CompanyProject> projects { get; set; }
        public string name { get; set; }
    }
}