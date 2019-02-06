using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SupportManagemantSystem.Models
{
    internal class MetaUser
    {
        public int userID { get; set; }
        [Display(Name = "نام کاربری")]
        [DisplayName("نام کاربری")]
        public string username { get; set; }
        [Display(Name = "کلمه عبور")]
        [DisplayName("کلمه عبور")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [DisplayName("نام و نام خانوادگی")]
        public string name { get; set; }
        [Display(Name = "گروه کاربری")]
        [DisplayName("گروه کاربری")]
        public Nullable<int> userGroupID { get; set; }
        [Display(Name = "ایمیل")]
        [DisplayName("ایمیل")]
        public string email { get; set; }


    }
    [MetadataType(typeof(MetaUser))]
    public partial class User
    {
    }

    internal class MetaUserGroup
    {

        [Display(Name = "گروه کاربری")]
        [DisplayName("گروه کاربری")]
        public string name { get; set; }
        [Display(Name = "گروه کاربری")]
        [DisplayName("گروه کاربری")]
        public int userGroupID { get; set; }
    }
    [MetadataType(typeof(MetaUserGroup))]
    public partial class UserGroup
    {

    }

    internal class MetaCompany
    {
        [Display(Name = "نام شرکت")]
        [DisplayName("نام شرکت")]
        public string name { get; set; }
        [Display(Name = "شماره تماس")]
        [DisplayName("شماره تماس")]
        public string phone { get; set; }
    }
    [MetadataType(typeof(MetaCompany))]
    public partial class Company { }


    internal class MetaTicket
    {
        public long ticketID { get; set; }
        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        public string title { get; set; }
        public Nullable<byte> statusID { get; set; }
        [Display(Name = "شماره تماس")]
        [DisplayName("شماره تماس")]
        public string callNumber { get; set; }
        public Nullable<int> projectID { get; set; }
    }
    [MetadataType(typeof(MetaTicket))]
    public partial class Ticket
    {
    }
    internal class MetaRequest
    {
        public long ticketID { get; set; }
        [Display(Name = "شرح")]
        [DisplayName("شرح")]
        public string text { get; set; }
        public Nullable<long> askerID { get; set; }

    }
    [MetadataType(typeof(MetaRequest))]
    public partial class Request
    {
    }
    internal class MetaResponse
    {

        [Display(Name = "پاسخ")]
        [DisplayName("پاسخ")]
        public string answer { get; set; }
        public int? answererID { get; set; }

    }
    [MetadataType(typeof(MetaResponse))]
    public partial class Response
    {
    }
    internal class MetaTask
    {
        public long taskID { get; set; }
        [Display(Name = "توضیح")]
        [DisplayName("توضیح")]
        public string decription { get; set; }
        [Display(Name = "تاریخ")]
        [DisplayName("تاریخ")]
        public string date { get; set; }
        [Display(Name = "زمان تخصیص یافته")]
        [DisplayName("زمان تخصیص یافته")]
        [Required(ErrorMessage = "زمان تخصیص یافته اجباری ست")]
        public double? spentTime { get; set; }
        public long? ticketID { get; set; }
        public int? projectID { get; set; }
        public int? companyID { get; set; }
    }
    [MetadataType(typeof(MetaTask))]
    public partial class Task
    {
    }
    internal class MetaProject
    {
        [DisplayName("نام پروژه")]
        [Display(Name = "نام پروژه"), Required]
        public string name { get; set; }
    }
    [MetadataType(typeof(MetaProject))]
    public partial class Project
    {

    }
    internal class MetaDuty
    {

        [Display(Name = "توضیح")]
        [DisplayName("توضیح")]
        public long dutyID { get; set; }
        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        public string title { get; set; }
        [Display(Name = "توضیح")]
        [DisplayName("توضیح")]
        public string description { get; set; }
        [Display(Name = "زمان پیش بینی شده")]
        [DisplayName("زمان پیش بینی شده")]
        public Nullable<int> requiredTime { get; set; }
        [Display(Name = "اولویت")]
        [DisplayName("اولویت")]
        public byte? piorityID { get; set; }
        [Display(Name = "پایان یافته")]
        [DisplayName("پایان یافته")]
        public bool? done { get; set; }
        public long? requestID { get; set; }
        public int? registerID { get; set; }
        public int? referedToUserID { get; set; }
        public int? projectID { get; set; }
    }
    [MetadataType(typeof(MetaDuty))]
    public partial class Duty
    {

    }
    internal class MetaMessage
    {
        [Display(Name = "متن پیام")]
        [DisplayName("متن پیام")]
        [Required(ErrorMessage = "ضروری است")]
        public string text { get; set; }
        [Display(Name = "عنوان")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "ضروری است")]
        public string title { get; set; }
    }
    [MetadataType(typeof(MetaMessage))]
    public partial class Message
    {
    }
    internal class MetaQuery
    {
        public long queryID { get; set; }
        [Display(Name = "کوئری")]
        [DisplayName("کوئری")]
        [Required(ErrorMessage = "ضروری است")]
        public string queryCommand { get; set; }
        [Display(Name = "تاریخ ثبت")]
        [DisplayName("تاریخ ثبت")]
        public DateTime? date { get; set; }
        [Display(Name = "نوع")]
        [DisplayName("نوع")]
        public byte? type { get; set; }
        [JsonIgnore]
        public virtual ICollection<Execute> Executes { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual QueryType QueryType { get; set; }
    }
    [MetadataType(typeof(MetaQuery))]
    public partial class Query
    {
    }
    internal class MetaQueryType
    {
        [Display(Name = "نوع")]
        [DisplayName("نوع")]
        public string name { get; set; }


    }
    [MetadataType(typeof(MetaQueryType))]
    public partial class QueryType
    {

    }
    internal class MetaError
    {
        [Display(Name = "کد خطا")]
        [DisplayName("کد خطا")]
        public string errorCodes { get; set; }
        public bool? done { get; set; }
    }
    [MetadataType(typeof(MetaError))]
    public partial class Error
    {

    }
    internal class MetaEXEFile
    {
        public long exeID { get; set; }
        [Display(Name = "نسخه")]
        [DisplayName("نسخه")]
        [Required(ErrorMessage = "درج نسخه ضروری است")]
        public string version { get; set; }
        [Display(Name = "تغییرات")]
        [DisplayName("تغییرات")]
        [Required(ErrorMessage = "درج تغییرات ضروری است")]
        public string changes { get; set; }
        [Display(Name = "تاریخ")]
        [DisplayName("تاریخ")]
        public DateTime? date { get; set; }
    }
    [MetadataType(typeof(MetaEXEFile))]
    public partial class EXEFile
    {
    }
}