using System;
using System.Web;

namespace UPTax.Helper
{
    public static class RapidSession
    {
        private const string roleId = "RoleId";
        private const string userId = "UserId";
        private const string roleName = "RoleName";
        private const string userFullName = "UserFullName";
        private const string con = "Con";
        private const string unionId = "UnionId";
        private const string notice = "Notice";
        private const string financialYearId = "FinancialYearId";

        private const string dateTimeFormat = "DateTimeFormat";
        public static string RoleId
        {
            get
            {
                return (string)(HttpContext.Current.Session[roleId] ?? "");
            }

            set
            {
                HttpContext.Current.Session[roleId] = value;
            }
        }
        public static string UserId
        {
            get
            {
                return (string)(HttpContext.Current.Session[userId] ?? "");
            }

            set
            {
                HttpContext.Current.Session[userId] = value;
            }
        }

        public static string Notice
        {
            get
            {
                return (string)(HttpContext.Current.Session[notice] ?? "");
            }

            set
            {
                HttpContext.Current.Session[notice] = value;
            }
        }
        public static string RoleName
        {
            get
            {
                return (string)(HttpContext.Current.Session[roleName] ?? "");
            }

            set
            {
                HttpContext.Current.Session[roleName] = value;
            }
        }

        public static string UserFullName
        {
            get
            {
                return (string)(HttpContext.Current.Session[userFullName] ?? "");
            }

            set
            {
                HttpContext.Current.Session[userFullName] = value;
            }
        }

        public static string Con
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session[con] == null)
                        throw new Exception();
                    return (string)HttpContext.Current.Session[con];
                }
                catch (Exception)
                {


                    return "";
                }

            }
            set
            {
                HttpContext.Current.Session[con] = value;
            }
        }

        public static string DateTimeFormat
        {
            get
            {
                return (string)HttpContext.Current.Session[dateTimeFormat];
            }
            set
            {
                HttpContext.Current.Session[dateTimeFormat] = value;
            }
        }

        public static int UnionId
        {
            get
            {
                try
                {
                    return (int)HttpContext.Current.Session[unionId];
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session[unionId] = value;
            }
        }
        public static int FinancialYearId
        {
            get
            {
                return (int)(HttpContext.Current.Session[financialYearId] ?? 1);
            }

            set
            {
                HttpContext.Current.Session[financialYearId] = value;
            }
        }
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
            //HttpContext.Current.Session.Abandon();
        }
    }
}
