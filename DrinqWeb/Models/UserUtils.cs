using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DrinqWeb.Models
{
    public class UserUtils
    {
        public static string GetAuthorizationStringFromHeader(HttpRequestMessage req)
        {
            return req.Headers.GetValues("Authorization").ToList().Count > 0 ?
                req.Headers.GetValues("Authorization").ToList()[0] : null;
        }


        public static ApplicationUser GetUserById(string Id)
        {
            if (Id == null)
                return null;
            if (Id.Length < 3)
                return null;
            using (ApplicationDbContext db = new ApplicationDbContext())
                return db.Users.Where(e => e.Id == Id).FirstOrDefault(); ;
        }

    }
}