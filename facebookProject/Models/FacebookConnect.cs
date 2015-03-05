using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using Facebook;

namespace facebookProject.Models
{

    public class FacebookConnect
    {

        public void GetFacebookUser()
        {
            var client = new FacebookClient();
            dynamic me = client.Get("zuck");
            string firstName = me.first_name;
            string lastName = me.last_name;
        }

    }

}
