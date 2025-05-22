using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBasketDev.Data;
using static WebBasketDev.Controllers.CustomersController;

namespace WebBasketDev.Controllers
{
    public class UserController : Controller
    {
        private BasketDBEntities db = new BasketDBEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ValidateUserLogin(string userName, string password)
        {
            try
            {
                var isValideUserSP = db.Database.SqlQuery<ResultModel>(
               "EXEC ValidateUser @UserName, @Password",
               new System.Data.SqlClient.SqlParameter("@UserName", userName),
               new System.Data.SqlClient.SqlParameter("@Password", password)
           ).FirstOrDefault();
                if (isValideUserSP == null)
                {
                    return Json(new { success = false, message = "Invalid username or password." });
                }
                else
                {
                    Session["UserName"] = isValideUserSP.UserName;
                    Session["UserId"] = isValideUserSP.Id;
                    Session["Role"] = isValideUserSP.Role;
                    return Json(new { success = true, message = "Login successful!" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}