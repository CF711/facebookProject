using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Facebook;
using facebookProject.Models;
using System.Xml.Linq;
namespace facebookProject.Controllers
{
    public class HomeController : Controller
    {
        private NosebookContext db = new NosebookContext();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //string data =  getStockData("name", "a");
            if (Session["AccessToken"] != null)
            {

                var accessToken = Session["AccessToken"].ToString();
                FacebookClient fb = new FacebookClient(accessToken);
                dynamic user = fb.Get("me");
                //Get list of stocks
                db.Transactions.ToList().Where(tr => tr.user_id == user.id).OrderBy(tr => tr.datetime);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void UserInfo(string accessToken)
        {
            var client = new FacebookClient(accessToken);
            dynamic result = client.Get("me");     
            createUser(result.id, result.first_name, result.last_name, result.email);
            Session["AccessToken"] = accessToken;
        }

        
        
        public User createUser(string id, string first_name, string last_name,
            string email)
        {
            User user = new User();
            user.user_id = id;
            user.first_name = first_name;
            user.last_name = last_name;
            user.fb_username = email;
            if (db.Users.ToList().Where(p => p.user_id == id).Count() == 0)
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            return user;
        }
        
    }
}
