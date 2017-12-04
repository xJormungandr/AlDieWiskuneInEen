using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoidConFormwork.Controllers
{
    public class ValidationController : Controller
    {
        [HttpPost]
        public bool EmailExist(string Email)
        {
            using (var check = new db_VoidConFormworkEntities())
            {

                var checkEmail = check.db_Users.Where(m => m.Email == Email).FirstOrDefault();

                if (checkEmail == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }



            }
        }

    }
}
