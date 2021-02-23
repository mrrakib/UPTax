﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPTax.Helper
{
    public static class RapidSession
    {
        private const string roleId = "RoleId";
        private const string roleName = "RoleName";
        private const string con = "Con";
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

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
            //HttpContext.Current.Session.Abandon();

        }
    }
}