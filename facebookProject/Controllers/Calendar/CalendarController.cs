using System;
using System.Linq;
using System.Web.Mvc;

namespace NoseBook.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult WeekView()
        {
            return View();
        }

        public void displayDayView()
        {
            Console.WriteLine("Called displayDayView");
        }

        public void displayWeekView()
        {

            Console.WriteLine("Called displayWeekView");
        }

        public void displayMonthView()
        {

            Console.WriteLine("Called displayMonthView");
        }


    }
}
