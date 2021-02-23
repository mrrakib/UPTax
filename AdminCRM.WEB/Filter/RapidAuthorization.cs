using UPTax.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UPTax.Filter
{
    public class RapidAuthorization : AuthorizeAttribute
    {
        bool IsSessionExist = false;
        public bool All { get; set; }

        #region UnauthorizedRequest
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (IsSessionExist == false)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.End();
                    //filterContext.HttpContext.Response.Write("<script>alert('Login time out! Please Loggin again!');window.location.href='../MasterPanel/Account/Login';</script>");
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Index" }, { "ReturnUrl", filterContext.HttpContext.Request.FilePath } });
                }

            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.Write("<script>UnAuthorized()</script>");
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "UnAuthorized" } });
                }

            }

        }
        #endregion

        #region AuthorizeCore
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (string.IsNullOrEmpty(RapidSession.Con))
            {
                IsSessionExist = false;
                return false;
            }
            else
            {
                if (RapidSession.RoleName == "Super Admin")
                {
                    return true;
                }
                else
                {

                    //SaveAudit(httpContext, controller, action);
                    if (All)
                    {
                        return true;
                    }
                    else
                    {
                        
                        IsSessionExist = true;
                        return false;
                    }

                }


            }

        }
        #endregion
    }
}