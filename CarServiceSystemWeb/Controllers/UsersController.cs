using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarServiceSystemWeb.EntityContext;

namespace CarServiceSystemWeb.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private CarServiceContext db = new CarServiceContext();
        List<string> Colours = new List<string>();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            List<Brand> brands = db.Brands.ToList();

            foreach (System.Reflection.PropertyInfo prop in typeof(System.Windows.Media.Colors).GetProperties())
            {
                Colours.Add(prop.Name);
            }

            
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name");
            ViewBag.ModelID = new SelectList(new List<string> { });
            ViewBag.Colour = new SelectList(Colours);

            return View(user);
        }

        public PartialViewResult AddCarPartialView()
        {
            //ModelState.AddModelError("AddUserViewModel", "Some Error.");

            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name");
            ViewBag.ModelID = new SelectList(new List<string> { });
            foreach (System.Reflection.PropertyInfo prop in typeof(System.Windows.Media.Colors).GetProperties())
            {
                Colours.Add(prop.Name);
            }
            
            ViewBag.Colour = new SelectList(Colours);
            return PartialView("AddCarPartialView", new Car());
        }

        [HttpPost]
        public JsonResult AddCar([Bind(Include = "Id,WINnumber,RegNumber,BrandID,ModelID,Colour")] Car car)
        {
            car.UserID = Int32.Parse(Request.Form.Get("userID"));
            
            bool isSuccess = false;
            string p = "";
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                isSuccess = true;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        p += error.ErrorMessage +" "+"newLineStr";
                    }
                }
            }
          //  Messagebox(p);
            return Json(new {Error = p }, JsonRequestBehavior.AllowGet);
        }

     /*   [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creat([Bind(Include = "Id,WINnumber,RegNumber,BrandID,ModelID,Colour")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return PartialView("Details_CreateCar",car);
            }
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name",car.Brand);
            ViewBag.ModelID = new SelectList(db.Models.Where(i=>i.BrandID==car.BrandID),car.Model);
            ViewBag.Colour = new SelectList(Colours);
            return Json(PartialView("Details_CreateCar", car));

        }*/

        [HttpPost]
        public ActionResult CreateCar(int brandID, int modelID, string winNumber, string regNumber, string colour, int userID)
        {
            Car c = new Car();

            c.BrandID = db.Brands.Where(i => i.Id == brandID).Select(i => i.Id).First();
            c.ModelID = db.Models.Where(i => i.Id == modelID).Select(i => i.Id).First();
            c.WINnumber = winNumber;
            c.RegNumber = regNumber;
            c.Colour = colour;
            c.UserID = userID;
            db.Cars.Add(c);
            db.SaveChanges();

            User user = db.Users.Find(userID);

            return Json(Url.Action("Details", "Users", user));
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surename,IdCardNumber,EGN,Address,Gender,PhoneNumber,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surename,IdCardNumber,EGN,Address,Gender,PhoneNumber,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            foreach(var c in db.Cars)
            {
                if (c.UserID == id)
                {
                    db.Cars.Remove(c);
                } 
            }
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
        public void Messagebox(string xMessage)
        {
            Response.Write(xMessage );
        }
    }
}
