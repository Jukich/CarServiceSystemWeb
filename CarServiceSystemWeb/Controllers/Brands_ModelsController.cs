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
    public class Brands_ModelsController : Controller
    {
        // GET: Brands_Models
        public ActionResult Index()
        {
            return View();
        }
        [Route("Brands")]
        public ActionResult Brands()
        {
            List<Brand> BrandList = new List<Brand>();
            using (var context = new CarServiceContext())
            {
                BrandList = context.Brands.ToList();
            }
            return View(BrandList);
        }

        [Route("Brands/{id}")]
        public ActionResult Models(int id)
        {
            List<Model> ModelList = new List<Model>();
            using (var context = new CarServiceContext())
            {
                ModelList = context.Models.Where(i => i.BrandID == id).ToList();
            }
            return View(ModelList);

        }

        public ActionResult Create(string name)
        {
            var file = Request.Files[0];
            
            if (file.FileName == null || file.FileName == "" || file==null)
            {
                return RedirectToAction("About","Home");
            }
            byte[] binaryImage;

            Stream stream = file.InputStream;
            {
                binaryImage = new byte[stream.Length];
                stream.Read(binaryImage, 0, (int)stream.Length);
            }

            var context = new CarServiceContext();
            var br = new Brand();


            br.Name = name;
            br.imageUrl = binaryImage;
            if (ModelState.IsValid)
            {
                context.Brands.Add(br);
                context.SaveChanges();

                var BrandList = context.Brands.ToList();
                return RedirectToAction("Brands",BrandList);
            }
            return RedirectToAction("Brands");
        }

        public BitmapImage ToImage(byte[] url)
        {
            using (var ms = new MemoryStream(url))
            {
                try
                {
                   
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    return image;
                }
                catch
                {
                    throw new Exception();
                }
            }
        }
    }
}