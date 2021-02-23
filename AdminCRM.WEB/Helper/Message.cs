using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UPTax.Helper
{
    public class Message
    {
        public void save(Controller controller)
        {

            controller.TempData["save"] = "1";
        }
        public void update(Controller controller)
        {

            controller.TempData["update"] = "1";
        }
        public void delete(Controller controller)
        {

            controller.TempData["delete"] = "1";
        }
        public void custom(Controller controller, string text)
        {

            controller.TempData["custom"] = text;
        }

        public void warning(Controller controller, string text)
        {

            controller.TempData["warning"] = text;
        }

        public void success(Controller controller, string text)
        {

            controller.TempData["success"] = text;
        }
    }
}