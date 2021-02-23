using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UPTax.Utility
{
    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var val = (string)value;
                if (!string.IsNullOrEmpty(val))
                    val = val.Trim();

                value = val;
            }

            base.SetProperty(controllerContext, bindingContext,
                propertyDescriptor, value);
        }
    }
}