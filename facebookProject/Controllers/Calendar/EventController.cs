using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using DayPilot.Web.Mvc.Json;
using facebookProject.Models;
using facebookProject.Controllers.Calendar;
using Facebook;

public class EventController : Controller
{
    FacebookClient fb = null;

    public EventController()
    {
        if (Session != null)
        {
            if (Session["AccessToken"] != null)
            {
                fb = new FacebookClient(Session["AccessToken"].ToString());
            }
        }
    }
    public ActionResult Edit(string id)
    {
        var e = new EventManager().Get(id) ?? new Event();
        return View(e);
    }


    [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
    public void Edit(FormCollection form)
    {
        if (form["command"] == "edit")
        {
            String name = form["name"];
            DateTime start = Convert.ToDateTime(form["Start"]);
            DateTime end = Convert.ToDateTime(form["End"]);
            new EventManager().EventEdit(form["Id"], name, start, end);
        }
        else if (form["command"] == "delete")
        {
            new EventManager().EventDelete(form["Id"]);
        }
        
    }

    public void Delete(String id)
    {
        new EventManager().EventDelete(id);
    }


    public ActionResult Create()
    {
        return View(new Event()
        {
            eventstart = Convert.ToDateTime(Request.QueryString["start"]),
            eventend = Convert.ToDateTime(Request.QueryString["end"])
        });
    }

    [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Create(FormCollection form)
    {
        dynamic user = fb.Get("me");
        DateTime start = Convert.ToDateTime(form["Start"]);
        DateTime end = Convert.ToDateTime(form["End"]);
        new EventManager().EventCreate(start, end, form["Text"], user.id);
        return JavaScript(SimpleJsonSerializer.Serialize("OK"));
    }
}
