using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CarServiceSystemWeb.EntityContext;

namespace CarServiceSystemWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {

            if (Session["UserID"] != null)
            {
                return RedirectToAction("About", "Home");
            }
            return View();
        }

        UserApplication userApp = new UserApplication();
        SessionContext context = new SessionContext();

        [HttpPost]
        public ActionResult Login(UserProfile user)
        {
        
            if (Session["UserID"]!=null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            var authenticatedUser = userApp.GetByUsernameAndPassword(user);
            if (authenticatedUser != null)
            {
                context.SetAuthenticationToken(authenticatedUser.UserId.ToString(), false, authenticatedUser);
                Session["UserID"] = authenticatedUser.UserId.ToString();
                Session["UserName"] = authenticatedUser.UserName.ToString();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewData["LogInError"] = "Wrong username or password";
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();        
            Session["UserID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");
        }
        public PartialViewResult SignUpPartialView()
        {

            return PartialView("SignUpPartialView", new UserProfile());
        }

        public ActionResult SignUp([Bind(Include = "UserName,Password")] UserProfile userProfile)
        {
            
            using (var context = new CarServiceSystemWeb.EntityContext.CarServiceContext())
            {
                if (ModelState.IsValid)
                {
                    context.UserProfiles.Add(userProfile);
                    context.SaveChanges();
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    string p = "NoErrorFound";
                    //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    if (ViewData.ModelState.Values.Count != 0)
                    {
                        p = null;
                    }
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            p += error.ErrorMessage + "\n";
                        }
                    }
                    return Json(new {Error = p }, JsonRequestBehavior.AllowGet);
                }
            }

            return PartialView("SignUpPartialView", userProfile);
        }

    }
}