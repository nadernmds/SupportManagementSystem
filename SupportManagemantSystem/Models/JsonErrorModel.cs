using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SupportManagemantSystem.Models
{
    public class JsonErrorModel
    {
        public JsonErrorModel()
        {
            Errors = new List<ErrorCode>();
            Done = new List<long>();
        }
        public List<ErrorCode> Errors { get; set; }
        public List<long> Done { get; set; }
        public DateTime date { get; set; }
        public string DataBaseName { get; set; }
        public string ConnectionString { get; set; }
        public int companyID { get; set; }
        public int projectID { get; set; }
        public class ErrorCode
        {
            public long id { get; set; }
            public string Error { get; set; }
        }
    }
}