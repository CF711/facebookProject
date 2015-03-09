using System;
using System.Linq;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;

namespace NoseBook.Controllers
{
    public class CalendarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }

        public ActionResult Week()
        {
            return View();
        }

        public ActionResult Month()
        {
            return View();
        }
        class Dpc : DayPilotCalendar
        {
            //DataClasses1DataContext db = new DataClasses1DataContext();

            protected override void OnInit(InitArgs e)
            {
                Update(CallBackUpdateType.Full);
            }

        //    protected override void OnEventResize(EventResizeArgs e)
        //    {
        //        //var toBeResized = (from ev in db.Events where ev.id == Convert.ToInt32(e.Id) select ev).First();
        //        //toBeResized.eventstart = e.NewStart;
        //        //toBeResized.eventend = e.NewEnd;
        //        //db.SubmitChanges();
        //        Update();
        //    }

        //    protected override void OnEventMove(EventMoveArgs e)
        //    {
        //        //var toBeResized = (from ev in db.Events where ev.id == Convert.ToInt32(e.Id) select ev).First();
        //        //toBeResized.eventstart = e.NewStart;
        //        //toBeResized.eventend = e.NewEnd;
        //        //db.SubmitChanges();
        //        Update();
        //    }

        //    protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
        //    {
        //        //var toBeCreated = new Event { eventstart = e.Start, eventend = e.End, text = (string)e.Data["name"] };
        //        //db.Events.InsertOnSubmit(toBeCreated);
        //        //db.SubmitChanges();
        //        Update();
        //    }

        //    protected override void OnFinish()
        //    {
        //        if (UpdateType == CallBackUpdateType.None)
        //        {
        //            return;
        //        }

        //        //Events = from ev in db.Events select ev;

        //        DataIdField = "id";
        //        DataTextField = "text";
        //        DataStartField = "eventstart";
        //        DataEndField = "eventend";
        //    }

        }

    }
}
