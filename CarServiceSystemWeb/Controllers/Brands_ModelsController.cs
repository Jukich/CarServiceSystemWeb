using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarServiceSystemWeb.EntityContext;
using System.Windows.Media.Imaging;
using System.IO;

namespace CarServiceSystemWeb.Controllers
{
    [Authorize]
    public class Brands_ModelsController : Controller
    {
        private CarServiceContext context = new CarServiceContext();

        // GET: Brands_Models
        public ActionResult Index()
        {
            return View();
        }

        [Route("Brands")]
        public ActionResult Brands()
        {
            List<Brand> BrandList = new List<Brand>();
            BrandList = context.Brands.ToList();
            ViewData["NameError"] = TempData["NameError"];
            ViewData["LogoError"] = TempData["LogoError"];
            return View(BrandList);
        }

        public ActionResult Create(string name)
        {
            string errorMessage;
            bool flag = false;

            if (!string.IsNullOrEmpty(errorMessage = Validation.Validate(name, "Brand")))
            {
                flag = true;
                TempData["NameError"] = errorMessage;
            }

            else
            {
                foreach (Brand bramd in context.Brands)
                {
                    if (bramd.Name == name)
                    {
                        TempData["NameError"] = "Brand with this name already exists";
                    }
                }
            }
 
            var file = Request.Files[0];

            if (file.FileName == null || file.FileName == "" || file == null)
            {
                TempData["LogoError"] = "Choose image";
                flag = true;
            }
            if (flag == true)
            {
                return RedirectToAction("Brands");
            }

            byte[] binaryImage;

            Stream stream = file.InputStream;
            {
                binaryImage = new byte[stream.Length];
                stream.Read(binaryImage, 0, (int)stream.Length);
            }

            var br = new Brand();
            br.Name = name;
            br.imageUrl = binaryImage;

            context.Brands.Add(br);
            context.SaveChanges();

            var BrandList = context.Brands.ToList();

            return RedirectToAction("Brands");
        }

        [Route("Brands/{id}")]
        public ActionResult Models(int id)
        {
            Brand br;
            List<Model> models;
            br = context.Brands.Find(id);
            models = context.Models.Where(i => i.BrandID == id).ToList();

            ViewBag.Models = models;
            ViewData["NewModelError"] = TempData["NewModelError"];
            ViewData["DeleteModelError"] = TempData["DeleteModelError"];
            ViewData["BrandNameError"] = TempData["BrandNameError"];
            return View(br);

        }


        public ActionResult EditBrand(int actionType, int brandID, string name)
        {
            Brand brand = context.Brands.Where(i => i.Id == brandID).First();

            string errorMessage;
            if(!string.IsNullOrEmpty(errorMessage=Validation.Validate(name, actionType==(1)?"Model":"Brand")))
            {
                string errorType = actionType == 1 ? "NewModelError" : actionType == 2 ? "DeleteModelError" : "BrandNameError";
                TempData[errorType] = errorType == "DeleteModelError" ? "Model with this name does not exist for current brand" : errorMessage;
                return Json(Url.Action("Models", "Brands_Models", brand));
            }

            if (actionType == 1)
            {
                foreach(Model md in context.Models)
                {
                    if (md.Name == name)
                    {
                        TempData["NewModelError"] = "Model with this name already exists";
                        return Json(Url.Action("Models",brand));
                    }
                }
                context.Models.Add(new Model { BrandID = brandID, Name = name });
                context.SaveChanges();
                return Json(Url.Action("Models", "Brands_Models", brand));

            }
            if (actionType == 2)
            {
                bool flag = false;
                foreach (Model md in context.Models.Where(i=>i.BrandID==brandID))
                {
                    if (md.Name == name)
                    {
                        var model = context.Models.Where(i => i.Name == name).First();
                        context.Models.Remove(model);
                        flag = true;
                    }
                }
                if (flag == true)
                {
                    context.SaveChanges();
                    return Json(Url.Action("Models", "Brands_Models", brand));
                }
                else
                {
                    TempData["DeleteModelError"] = "Model with this name does not exist";
                    return Json(Url.Action("Models", "Brands_Models", brand));
                }

            }
            if (actionType == 3)
            {
                foreach(Brand br in context.Brands)
                {
                    if (name == br.Name)
                    {
                        TempData["BrandNameError"] = "Brand with this name already exists";
                        return Json(Url.Action("Models", "Brands_Models", brand));
                    }
                }
                brand.Name = name;
                context.SaveChanges();

                brand = context.Brands.Find(brandID);
                return Json(Url.Action("Models", "Brands_Models", brand));
            }
            if (actionType == 4)
            {
                foreach (Model md in context.Models)
                {
                    if (md.BrandID == brandID)
                        context.Models.Remove(md);
                }

                context.Brands.Remove(brand);
                context.SaveChanges();
                var BrandList = context.Brands.ToList();
                return Json(Url.Action("Brands", "Brands_Models", BrandList));
            }
            if (actionType == 5)
            {  
                var file = Request.Files[0];

                if (file.FileName == null || file.FileName == "" || file == null)
                {
                    return RedirectToAction("About", "Home");
                }
                byte[] binaryImage;

                Stream stream = file.InputStream;
                {
                    binaryImage = new byte[stream.Length];
                    stream.Read(binaryImage, 0, (int)stream.Length);
                }
                brand.imageUrl = binaryImage;
                context.SaveChanges();

                return  RedirectToAction("Models", "Brands_Models", brand);

            }
            return Json(Url.Action("About", "Home"));

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}