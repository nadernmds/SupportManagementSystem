using SupportManagemantSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupportManagemantSystem
{
    public class RequsetLoginAttribute : AuthorizeAttribute
    {
        private int[] UserGroupID;
        public RequsetLoginAttribute(params int[] UserGroupID)
        {
            this.UserGroupID = UserGroupID;
        }
        public RequsetLoginAttribute()
        {

        }
        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return base.OnCacheAuthorization(httpContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (UserGroupID != null)
            {
                var user = (User)httpContext.Session["RPG"] ?? new User();
                return httpContext.Session["RPG"] != null && UserGroupID.Contains(user.userGroupID.Value);
            }
            return (httpContext.Session["RPG"] != null);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/user/login");
        }
    }
}