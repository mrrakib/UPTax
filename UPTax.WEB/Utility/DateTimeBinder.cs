using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using UPTax.Helper;

namespace UPTax.Utility
{
    public class DateTimeBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string actionName = controllerContext.RouteData.Values["action"].ToString();
            string controllerName = controllerContext.RouteData.Values["controller"].ToString();
            if (!actionName.Equals("BatchCreate") && !(controllerName.Equals("SalaryProcess") && actionName.Equals("SaveProcess")))
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                string val = "";
                if (value != null)
                {
                    val = value.AttemptedValue;

                    DateTime date;
                    var displayFormat = RapidSession.DateTimeFormat;
                    if (value.AttemptedValue.Contains(","))
                    {
                        val = value.AttemptedValue.Split(',')[0];
                    }
                    if (val != "")
                    {
                        if (val.Count() != 5)
                        {
                            if (DateTime.TryParseExact(val, displayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                            {
                                return date;
                            }
                            else
                            {
                                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid Date Format");
                            }
                        }
                        else
                        {
                            if (DateTime.TryParseExact(val, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                            {
                                return date;
                            }
                            else
                            {
                                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid Time Format");
                            }
                        }
                    }
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}