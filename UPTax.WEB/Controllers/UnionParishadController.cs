using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models.UnionDetails;
using UPTax.Model.ViewModels;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class UnionParishadController : Controller
    {
        #region Global Variables
        private Message _message = new Message();

        private readonly IUnionParishadService _unionParishadService;
        private readonly string subPath = "~/assets/UserImages";
        private readonly string uploadFileName = "up_";
        #endregion

        #region constructor
        public UnionParishadController(IUnionParishadService unionParishadService)
        {
            _unionParishadService = unionParishadService;
        }
        #endregion

        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;

            var unionList = _unionParishadService.GetPaged(name, page, dataSize);
            return View(unionList);
        }

        #region Create
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UnionParishad model)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _unionParishadService.IsExistingItem(model);
                var imageUpDetails = UploadImage(Request);
                model.ImagePath = imageUpDetails.ImagePath;
                model.ImageName = imageUpDetails.ImageName;
                if (!existingItem && _unionParishadService.Add(model))
                {
                    _message.save(this);
                    return View();
                }
                else
                {
                    _message.custom(this, "এই নামে একটি ইউনিয়ন পরিষদ আছে!");
                }
            }
            return View(model);
        }
        #endregion

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UnionParishad model = _unionParishadService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnionParishad model)
        {
            if (ModelState.IsValid)
            {

                var hasFile = Request.Files[0];
                if (hasFile != null && hasFile.ContentLength > 0)
                {
                    bool exists = Directory.Exists(Server.MapPath(subPath));
                    if (exists)
                    {
                        var imageName = String.IsNullOrEmpty(model.ImageName) ? "" : model.ImageName;
                        var filteredByFilename = Directory
                                .GetFiles(Server.MapPath(subPath))
                                .Select(f => Path.GetFileName(f))
                                .Where(f => f.Equals(imageName));

                        if (filteredByFilename != null)
                        {
                            foreach (var filname in filteredByFilename)
                            {
                                var path = Path.Combine(Server.MapPath(subPath), filname);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                            }

                        }
                    }
                    //delete previous file
                    var imageUpDetails = UploadImage(Request);
                    model.ImagePath = imageUpDetails.ImagePath;
                    model.ImageName = imageUpDetails.ImageName;
                }

                var existingItem = _unionParishadService.IsExistingItem(model);
                if (existingItem)
                {
                    _message.custom(this, "এই নামে একটি ইউনিয়ন পরিষদ আছে!");
                    return View(model);
                }
                _unionParishadService.Update(model);
                _message.update(this);
                return RedirectToAction("Index");
                //return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
            }
            return View(model);
        }
        #endregion

        #region Delete
        [RapidAuthorization]
        public ActionResult Delete(int id)
        {
            if (_unionParishadService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion

        #region Image Upload
        private VMImage UploadImage(HttpRequestBase httpRequest)
        {
            string filePath = "";
            string fileName = "";
            if (httpRequest.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(Request.Files["file"].FileName);
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        fileName = uploadFileName + Guid.NewGuid().ToString() + fileExt;

                        bool exists = Directory.Exists(Server.MapPath(subPath));
                        if (!exists)
                            Directory.CreateDirectory(Server.MapPath(subPath));

                        var path = Path.Combine(Server.MapPath(subPath), fileName);
                        file.SaveAs(path);
                        filePath = subPath + "/" + fileName;
                    }
                }
            }
            return new VMImage { ImageName = fileName, ImagePath = filePath };
        }
        #endregion
    }
}