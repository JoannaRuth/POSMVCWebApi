using POSMVCWebAPIClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace POSMVCWebAPIClient.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            if (Session["AssociateId"] != null)
                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult AddAssociate()
        {
            return View();
        }

        public ActionResult SaveAssociate(Associate a)
        {

            string apiURI = "http://153.59.21.26/POSMVCWebAPI/api/Manager";
            var client = new HttpClient();
            var values = new Dictionary<string, string>()
            {
                {"AssociateId",a.AssociateId.ToString() },
                {"AssociatePwd",a.AssociatePwd },
                {"RoleId",a.RoleId.ToString() }
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
                    return RedirectToAction("Index", "Manager");

                else
                {
                    //Could'nt add associate
                    ModelState.AddModelError(string.Empty, "server error.plase contact adminstrator");
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult RemoveAssociate(Associate a)
        {
            string apiURI = "http://153.59.21.26/POSMVCWebAPI/api/Manager/";
            apiURI = apiURI + a.AssociateId.ToString();
            var client = new HttpClient();



            var response = client.DeleteAsync(apiURI);


            // use try catch blocks to handle server errors...
            try
            {
                if (response.Result.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Manager");

                else
                {
                    //Could'nt delete associate
                    ModelState.AddModelError(string.Empty, "server error.plase contact adminstrator");
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        public ActionResult List()
        {
           string apiURI = "http://153.59.21.26/POSMVCWebAPI/api/Associates";
           //string apiURI = "http://localhost/BillingSystemWebAPI/api/Manager";
            var client = new HttpClient();

            var response = client.GetAsync(apiURI);


            List<Associate> AssociateList = new List<Associate>();

            if (response.Result.IsSuccessStatusCode)
            {
                var readTask = response.Result.Content.ReadAsAsync<List<Associate>>();
                var associates = readTask.Result;
                //var studsMarks =;
                foreach (var item in associates)
                {

                    AssociateList.Add(new Associate
                    {
                        AssociateId = item.AssociateId,
                        AssociatePwd = item.AssociatePwd,
                        RoleId = item.RoleId

                    });

                }
                return View(AssociateList);
            }
            else
            {
                //no associates found
                return View();
            }
        }

    }
}