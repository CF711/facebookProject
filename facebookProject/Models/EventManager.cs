using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace facebookProject.Models
{
    /// <summary>
    /// Summary description for EventManager
    /// </summary>
    public class EventManager
    {

        public NosebookContext db;

        public EventManager()
        {
            db = new NosebookContext();
        }
        public DataTable FilteredData(DateTime start, DateTime end)
        {

            List<Event> filteredDt = db.Events.Where(p => p.eventend <= end && p.eventstart >= start).ToList();
            DataTable dt = new DataTable();
            ArrayList array = new ArrayList();
            if (filteredDt.Count > 0)
            {
                dt.Columns.Add("event_id");
                dt.Columns.Add("name");
                dt.Columns.Add("eventstart");
                dt.Columns.Add("eventend");
                // Add the data from the Event Objects to the Data table
                foreach (Event row in filteredDt)
                {
                    DataRow dr = dt.NewRow();
                    dr["event_id"] = row.id;
                    dr["name"] = row.name;
                    dr["eventstart"] = row.eventstart;
                    dr["eventend"] = row.eventend;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            return dt;
        }

        public void EventEdit(string id, string name, DateTime start, DateTime end)
        {
            Event eventToUpdate = db.Events.Where(p => p.id == int.Parse(id)).SingleOrDefault<Event>();
            eventToUpdate.eventstart = start;
            eventToUpdate.eventend = end;
            eventToUpdate.name = name;
            db.SaveChanges();
        }

        public void EventMove(string id, DateTime start, DateTime end)
        {
            int intId = int.Parse(id);
            Event eventToUpdate = db.Events.Where(p => p.id == intId).SingleOrDefault<Event>();
            eventToUpdate.eventstart = start;
            eventToUpdate.eventend = end;
            db.SaveChanges(); 
            
        }

        public Event Get(string id)
        {
            Event eventToGet = db.Events.Where(p => p.id == int.Parse(id)).SingleOrDefault<Event>();
            return eventToGet;
        }

        public void EventCreate(DateTime start, DateTime end, string name)
        {
            Event newEvent = new Event();
            newEvent.eventstart = start;
            newEvent.eventend = end;
            newEvent.name = name;
            db.Events.Add(newEvent);
            db.SaveChanges();
        }


        public void EventDelete(string id)
        {
            Event eventToDelete = db.Events.Where(p => p.id == int.Parse(id)).SingleOrDefault<Event>();
            db.Events.Remove(eventToDelete);
            db.SaveChanges();
        }
    }

}