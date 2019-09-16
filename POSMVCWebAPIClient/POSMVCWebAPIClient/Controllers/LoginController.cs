using POSMVCWebAPIClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace POSMVCWebAPIClient.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Session["AssociateId"] = 0;
            return View();
        }

        [HttpPost]

        public ActionResult AssociateLogin(Associate a)
        {
            string apiURI = "http://153.59.21.26/POSMVCWebAPI/api/Associates";
            var client = new HttpClient();
            var values = new Dictionary<string, string>()
            {
                {"AssociateId",a.AssociateId.ToString() },
                {"AssociatePwd",a.AssociatePwd },
            };

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync(apiURI, content);
            string op = "";
            Associate newAssociate;

            // use try catch blocks to handle server errors...
            try
            {
                using (HttpContent cont = response.Result.Content)
                {
                    Task<string> res = cont.ReadAsStringAsync();
                    op = res.Result;

                    //User user=JsonConvert.DeserializeObject<User>
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    newAssociate = js.Deserialize<Associate>(op);

                }
                if (response.Result.IsSuccessStatusCode)
                {
                    // session create .

                    Session["AssociateId"] = newAssociate.AssociateId;

                    if (newAssociate.RoleId == 1)
                    {
                        return RedirectToAction("Index", "Associate");
                        // cashier

                    }
                    else if (newAssociate.RoleId == 2)
                    {
                        return RedirectToAction("Index", "Manager");
                        // faculty
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "server error.plase contact adminstrator");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "server error.plase contact adminstrator");
                    //respone 400

                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult Logout()
        {
            //Disable back button In all browsers.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}


