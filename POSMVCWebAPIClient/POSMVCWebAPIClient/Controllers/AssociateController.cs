using POSMVCWebAPIClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace POSMVCWebAPIClient.Controllers
{
    public class AssociateController : Controller
    {
        public static TransactionXml obj = new TransactionXml();
        public static List<Item> RetreivedItemList = new List<Item>();

        //Associate functions
        public ActionResult Index()
        {
            return View();
        }

        //Transaction function
        public ActionResult Transaction()
        {
            return View();
        }
        public ActionResult DisplayTransaction(string TransactionId)
        {
            Transaction transaction;

            string apiURI = "http://153.59.21.26/POSMVCWebAPI/api/Transactions/" + TransactionId;
            var client = new HttpClient();
            var response = client.GetAsync(apiURI);

            string op = "";

            if (response.Result.IsSuccessStatusCode)
            {
                using (HttpContent cont = response.Result.Content)
                {
                    Task<string> res = cont.ReadAsStringAsync();
                    op = res.Result;


                    JavaScriptSerializer js = new JavaScriptSerializer();

                    transaction = js.Deserialize<Transaction>(op);

                    XmlSerializer xs = new XmlSerializer(typeof(TransactionXml));
                    TextReader txtrdr = new StringReader(transaction.TransactionList);

                    obj = (TransactionXml)xs.Deserialize(txtrdr);
                    RetreivedItemList = obj.Item.ToList();
                }
                return View(RetreivedItemList);
            }
            else
            {
              
                return RedirectToAction("Index");

            }


        }
    }
}