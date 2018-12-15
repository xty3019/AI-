using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSS.Areas.CaiWuGuanLi.Models;

namespace PSS.Controllers
{
    public class LoginController : Controller
    {
        private MyEF EF = new MyEF();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetLoginData(Areas.CaiWuGuanLi.Models.Users Users)
        {
            if (Users.UsersID > 0)
            {
                return Json((from u in EF.Users
                             where u.UsersID == Users.UsersID
                             select new
                             {
                                 u.UsersName
                             }).ToList());
            }
            else
            {
                return Json((from u in EF.Users
                             where u.UserLoginName == Users.UserLoginName && u.UserLoginPwd == Users.UserLoginPwd
                             select new
                             {
                                 u.UsersID,
                                 u.UsersName,
                                 u.UserLoginName,
                                 u.UserLoginPwd
                             }).ToList());
            }
        }
    }
}