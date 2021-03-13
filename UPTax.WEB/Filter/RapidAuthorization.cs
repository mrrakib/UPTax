using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UPTax.Data;
using UPTax.Helper;

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
                        string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
                        string action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();

                        if (IsPermitted(controller, action))
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

        }
        #endregion

        #region Check Permissions
        private bool IsPermitted(string controllerName, string actionName)
        {

            if (HttpContext.Current.Session["ACL"] == null)
            {
                AdminContext db = new AdminContext(RapidSession.Con);
                int tag = 0;
                var sql = "SELECT COUNT(p.Id)total FROM MenuPermission p JOIN MenuConfigs c on c.Id=p.MenuConfigId WHERE c.ControllerName='" + controllerName + "' AND p.RoleId='" + RapidSession.RoleId + "' AND";
                if (actionName.ToLower() == "index")
                {
                    sql += " p.IsViewPermitted=1";
                    tag++;
                }
                else if (actionName.ToLower() == "create")
                {
                    sql += " p.IsAddPermitted=1";
                    tag++;
                }
                else if (actionName.ToLower() == "edit")
                {
                    sql += " p.IsEditPermitted=1";
                    tag++;
                }
                else if (actionName.ToLower() == "delete")
                {
                    sql += " p.IsDeletePermitted=1";
                    tag++;
                }
                else
                {
                    return false;
                }
                if (tag > 0)
                {
                    var total = db.Database.SqlQuery<Counter>(sql).FirstOrDefault();
                    if (total.total > 0)
                    {
                        return true;
                    }
                    return false;
                }

            }
            var AuthorList = (Dictionary<string, string>)HttpContext.Current.Session["ACL"];
            if (AuthorList.ContainsKey(controllerName.ToLower() + "_" + actionName.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
    public class Counter
    {
        public int total { get; set; }
    }
}