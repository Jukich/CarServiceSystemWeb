using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CarServiceSystemWeb.EntityContext;

namespace CarServiceSystemWeb.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private CarServiceContext db = new CarServiceContext();

        // GET: Cars
        public ActionResult Index()
        {
            var cars = db.Cars.Include(c => c.Brand).Include(c => c.Model).Include(c => c.User);
            return View(cars.ToList());
        }

        // GET: Cars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name");
            ViewBag.ModelID = new SelectList(db.Models, "Id", "Name");
            ViewBag.UserID = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,WINnumber,RegNumber,BrandID,ModelID,Colour,UserID")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name", car.BrandID);
            ViewBag.ModelID = new SelectList(db.Models, "Id", "Name", car.ModelID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "Name", car.UserID);
            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            List<string> Colours = new List<string>();
            Colours.Add(null);
            foreach (System.Reflection.PropertyInfo prop in typeof(System.Windows.Media.Colors).GetProperties())
            {
                Colours.Add(prop.Name);
            }

            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name", car.BrandID);
            ViewBag.ModelID = new SelectList(db.Models.Where(i=>i.BrandID==car.BrandID), "Id", "Name", car.ModelID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "Name", car.UserID);
            ViewBag.Colours = new SelectList(Colours, car.Colour);

            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,WINnumber,RegNumber,BrandID,ModelID,Colour,UserID")] Car car)
        {
            db.Entry(car).State = EntityState.Modified;

            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
               // throw new Exception();
                return RedirectToAction("Index","Users");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name", car.BrandID);
            ViewBag.ModelID = new SelectList(db.Models, "Id", "Name", car.ModelID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "Name", car.UserID);
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(int id)
        {
            Car c = db.Cars.Where(i => i.Id == id).First();
            var userID = c.UserID;
            db.Cars.Remove(c);
            db.SaveChanges();
            var user = db.Users.Where(i => i.Id == userID).First();

            return RedirectToAction("Details", "Users", user);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetModels(int id)
        {

            //var models ; 
            var context = new CarServiceSystemWeb.EntityContext.CarServiceContext();

            List<Model> models = new List<Model>();
            foreach (var mod in context.Models.Where(i => i.BrandID == id))
            {
                models.Add(new Model
                {
                    Id = mod.Id,
                    Name = mod.Name
                });
            }
          //  Messagebox("dd");
            var selectedList = new SelectList(models,"Id","Name");
            return Json(new SelectList(models, "Id", "Name"));
        }

        public void Messagebox(string xMessage)
        {
            Response.Write("<script>alert('" + xMessage + "')</script>");
        }
    }
}
