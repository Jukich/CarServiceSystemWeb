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
    public class UsersController : Controller
    {
        private CarServiceContext db = new CarServiceContext();

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
            List<string> Colours = new List<string>();

            Colours.Add(null);
            foreach (System.Reflection.PropertyInfo prop in typeof(System.Windows.Media.Colors).GetProperties())
            {
                Colours.Add(prop.Name);
            }

            
            ViewBag.BrandID = new SelectList(db.Brands, "Id", "Name",null);
            ViewBag.Colours = new SelectList(Colours, Colours[0]);

            return View(user);
        }

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

            return Json(Url.Action("Details", "Users",user));
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
    }
}
