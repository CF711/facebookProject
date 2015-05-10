using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Data;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
using facebookProject.Models;
using System.Data;
using Facebook;

namespace facebookProject.Controllers.Calendar
{
    public class BackEndController : Controller
    {
        //
        // GET: /BackEnd/

        FacebookClient fb = null; 

        public BackEndController()
        {
            
            
        }
        public ActionResult Month()
        {
            if (Session["AccessToken"] == null)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                var accessToken = Session["AccessToken"].ToString();
                fb = new FacebookClient(accessToken);
                return new Dpm(fb).CallBack(this);
            }
        }

        public ActionResult Week()
        {
            if (Session["AccessToken"] == null)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                var accessToken = Session["AccessToken"].ToString();
                fb = new FacebookClient(accessToken);
                Dpc dpc = new Dpc(fb);
                dpc.HeaderDateFormat = new DateTime().ToString("dddd");
                return dpc.CallBack(this);
            }
            
        }

        public ActionResult Day()
        {
            if (Session["AccessToken"] == null)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                var accessToken = Session["AccessToken"].ToString();
                fb = new FacebookClient(accessToken);
                Dpc dpc = new Dpc(fb);
                dpc.HeaderDateFormat = new DateTime().ToString("dddd");
                return dpc.CallBack(this);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        class Dpc : DayPilotCalendar
        {
            private FacebookClient fb;
            public Dpc( FacebookClient fb)
            {
                this.HeaderDateFormat = "dddd";
                this.fb = fb;
            }
            protected override void OnInit(InitArgs e)
            {
                Update(CallBackUpdateType.Full);

            }



            protected override void OnEventResize(EventResizeArgs e)
            {
                new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd);
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd);
                Update();
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                string name = (string)e.Data["name"];
                if (String.IsNullOrEmpty(name))
                {
                    name = "(default)";
                }
                dynamic user = fb.Get("me");
                new EventManager().EventCreate(e.Start, e.End, name, user.id);
                Update();
            }
            

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "navigate":
                        StartDate = (DateTime)e.Data["day"];
                        Update(CallBackUpdateType.Full);
                        break;

                    case "refresh":
                        Update(CallBackUpdateType.EventsOnly);
                        break;

                    case "delete":
                        new EventManager().EventDelete((string)e.Data["e"]["id"]);
                        Update(CallBackUpdateType.EventsOnly);
                        break;

                    case "previous":
                        StartDate = StartDate.AddDays(-7);
                        Update(CallBackUpdateType.Full);
                        break;

                    case "next":
                        StartDate = StartDate.AddDays(7);
                        Update(CallBackUpdateType.Full);
                        break;

                    case "day_next":
                        StartDate = StartDate.AddDays(1);
                        Update(CallBackUpdateType.Full);
                        break;

                    case "day_previous":
                        StartDate = StartDate.AddDays(-1);
                        Update(CallBackUpdateType.Full);
                        break;

                    case "today":
                        StartDate = DateTime.Today;
                        Update(CallBackUpdateType.Full);
                        break;
                }
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }
                dynamic user = fb.Get("me");
                //Events
                DataTable eventData = new EventManager().FilteredData(StartDate, StartDate.AddDays(Days), user.id);
                Events = convertEventDataTableToList(eventData);

                DataIdField = "id";
                DataTextField = "name";
                DataStartField = "eventstart";
                DataEndField = "eventend";
            }

            public List<Event> convertEventDataTableToList(DataTable eventData)
            {
                List<Event> eventList = new List<Event>();
                foreach(DataRow row in eventData.Rows){
                    Event newEvent = new Event();
                    newEvent.name = row["name"].ToString();
                    newEvent.id = int.Parse(row["event_id"].ToString());
                    newEvent.eventstart = DateTime.Parse(row["eventstart"].ToString());
                    newEvent.eventend = DateTime.Parse(row["eventend"].ToString());
                    eventList.Add(newEvent);
                }
                return eventList;
            }
        }

        class Dpm : DayPilotMonth
        {
            private FacebookClient fb;
            public Dpm( FacebookClient fb)
            {
                this.fb = fb;
            }
            protected override void OnInit(DayPilot.Web.Mvc.Events.Month.InitArgs e)
            {
                Update();
            }




            protected override void OnCommand(DayPilot.Web.Mvc.Events.Month.CommandArgs e)
            {
                switch (e.Command)
                {
                    case "navigate":
                        StartDate = (DateTime)e.Data["day"];
                        Update(CallBackUpdateType.Full);
                        break;
                    case "refresh":
                        Update(CallBackUpdateType.EventsOnly);
                        break;
                    case "delete":
                        new EventManager().EventDelete((string)e.Data["e"]["id"]);
                        Update(CallBackUpdateType.EventsOnly);
                        break;
                }
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }
                dynamic user = fb.Get("me");

                DataTable eventData = new EventManager().FilteredData(VisibleStart, VisibleEnd, user.id);
                Events = convertEventDataTableToList(eventData);
                DataIdField = "id";
                DataTextField = "name";
                DataStartField = "eventstart";
                DataEndField = "eventend";
            }

            public List<Event> convertEventDataTableToList(DataTable eventData)
            {
                List<Event> eventList = new List<Event>();
                foreach (DataRow row in eventData.Rows)
                {
                    Event newEvent = new Event();
                    newEvent.name = row["name"].ToString();
                    newEvent.id = int.Parse(row["event_id"].ToString());
                    newEvent.eventstart = DateTime.Parse(row["eventstart"].ToString());
                    newEvent.eventend = DateTime.Parse(row["eventend"].ToString());
                    eventList.Add(newEvent);
                }
                return eventList;
            }
        }

    }
}
