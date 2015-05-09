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

namespace facebookProject.Controllers.Calendar
{
    public class BackEndController : Controller
    {
        //
        // GET: /BackEnd/

        public ActionResult Month()
        {
            return new Dpm().CallBack(this);
        }

        public ActionResult Week()
        {
            Dpc dpc = new Dpc();
            dpc.HeaderDateFormat = new DateTime().ToString("dddd");
            return dpc.CallBack(this);
        }

        public ActionResult Day()
        {
            Dpc dpc = new Dpc();
            dpc.HeaderDateFormat = "dddd";
            return dpc.CallBack(this);
        }

        class Dpc : DayPilotCalendar
        {
            public Dpc()
            {
                this.HeaderDateFormat = "dddd";
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
                new EventManager().EventCreate(e.Start, e.End, "New event");
                Update();
            }

            //protected override void OnBeforeEventRender(BeforeEventRenderArgs e)
            //{
            //    e.Areas.Add(new Area().Right(3).Top(3).Width(15).Height(15).CssClass("event_action_delete").JavaScript("switcher.active.control.commandCallBack('delete', {'e': e});"));
            //}

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
                }
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                //Events
                DataTable eventData = new EventManager().FilteredData(StartDate, StartDate.AddDays(Days));
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
            protected override void OnInit(DayPilot.Web.Mvc.Events.Month.InitArgs e)
            {
                Update();
            }

            protected override void OnEventResize(DayPilot.Web.Mvc.Events.Month.EventResizeArgs e)
            {
                new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd);
                Update();
            }

            protected override void OnEventMove(DayPilot.Web.Mvc.Events.Month.EventMoveArgs e)
            {
                new EventManager().EventMove(e.Id, e.NewStart, e.NewEnd);
                Update();
            }

            protected override void OnTimeRangeSelected(DayPilot.Web.Mvc.Events.Month.TimeRangeSelectedArgs e)
            {
                new EventManager().EventCreate(e.Start, e.End, "New event");
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

            //protected override void OnFinish()
            //{
            //    if (UpdateType == CallBackUpdateType.None)
            //    {
            //        return;
            //    }

            //    Events = new EventManager().FilteredData(VisibleStart, VisibleEnd).AsEnumerable();

            //    DataIdField = "id";
            //    DataTextField = "name";
            //    DataStartField = "eventstart";
            //    DataEndField = "eventend";
            //}
        }

    }
}
