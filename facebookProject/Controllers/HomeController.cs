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
using PusherServer;
using System.Configuration;
namespace facebookProject.Controllers
{
    public class HomeController : Controller
    {
        
        private NosebookContext db = new NosebookContext();
        private static readonly Pusher provider = new Pusher
        (
           ConfigurationManager.AppSettings["pusher_app_id"],
           ConfigurationManager.AppSettings["pusher_key"],
           ConfigurationManager.AppSettings["pusher_secret"]
        );

        public void PostChat( string chatMessage )
        {
            var accessToken = HttpContext.ApplicationInstance.Request.Cookies["jumbleUP"].Value;
            FacebookClient fb = new FacebookClient(accessToken);
            dynamic user = fb.Get("me");
            if (!String.IsNullOrEmpty(chatMessage))
            {
                //Chat Part
                var now = DateTime.UtcNow;

                var result = provider.Trigger("test_channel", "message_received", new
                {
                    message = chatMessage,
                    user = user.first_name,
                    timestamp = now.ToShortDateString() + " " + now.ToShortTimeString()
                });
                //End Chat
            }
                
        }
        public ActionResult Index(string chatMessage)
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //string data =  getStockData("name", "a");
            try
            {

                var accessToken = HttpContext.ApplicationInstance.Request.Cookies["jumbleUP"].Value;
                FacebookClient fb = new FacebookClient(accessToken);
                dynamic user = fb.Get("me");
                //Get list of stocks

                //db.Transactions.ToList().Where(tr => tr.user_id == user.id).OrderBy(tr => tr.datetime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                
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

        public void UserInfo()
        {
            throw new Exception("AAA");
            var client = new FacebookClient( HttpContext.ApplicationInstance.Request.Cookies["jumbleUP"].Value);
            dynamic result = client.Get("me");     
            createUser(result.id, result.first_name, result.last_name, result.email);
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
