using POSMVCWebAPIClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class ItemController : Controller
    {
        public static List<Item> ItemList = new List<Item>();
        public static int i = 0;
        public static string itemAvailability = "";
        public static decimal totalAmount = 0;

        // GET: Item
        public ActionResult Index(string id)
        {


            ViewBag.message = itemAvailability;
            ViewBag.totalAmount = totalAmount;


            return View();
        }

       

        public ActionResult ScanItem()
        {

            return PartialView();

        }

        public ActionResult DisplayItemList()
        {

            return PartialView(ItemList);
        }



        public ActionResult Delete(string id)
        {
            bool alreadyExists = ItemList.Any(x => x.ItemId == id);
            if (alreadyExists)
            {
                Item itemFoundInList = ItemList.Find(itemsFoundInList => itemsFoundInList.ItemId.Equals(id));
                if (itemFoundInList.Quantity > 1)
                {
                    itemFoundInList.Quantity--;
                    itemFoundInList.PriceMultiplied = (itemFoundInList.Price) * itemFoundInList.Quantity;

                    //
                    totalAmount = totalAmount - itemFoundInList.Price;
                    //

                }

                else
                {
                    totalAmount = totalAmount - itemFoundInList.Price;
                    ItemList.Remove(itemFoundInList);
                }
            }

            return RedirectToAction("Index");
        }


        public ActionResult CheckItemAvailability(string ItemId) //parameter should be same as field name , not case sensitive
        {
            Item item;


            string op = "";

            bool alreadyExists = ItemList.Any(x => x.ItemId == ItemId);

            if (alreadyExists)
            {
                Item itemFoundInList = ItemList.Find(itemsFoundInList => itemsFoundInList.ItemId.Equals(ItemId));
                itemFoundInList.Quantity++;
                itemFoundInList.PriceMultiplied = (itemFoundInList.Price) * itemFoundInList.Quantity;

                //hello
                totalAmount = totalAmount + itemFoundInList.Price;
                //
                return RedirectToAction("Index");
            }

            else
            {
                // string apiURI = "http://localhost:53297/api/Items/";

                string apiURI = ConfigurationManager.AppSettings["apiGetUri"];

                apiURI = apiURI + ItemId;
                var client = new HttpClient();

                var response = client.GetAsync(apiURI);
                if (response.Result.IsSuccessStatusCode)
                {
                    using (HttpContent cont = response.Result.Content)
                    {
                        Task<string> res = cont.ReadAsStringAsync();
                        op = res.Result;

                        //User user=JsonConvert.DeserializeObject<User>
                        JavaScriptSerializer js = new JavaScriptSerializer();

                        item = js.Deserialize<Item>(op);

                    }

                    item.Index = i;
                    item.PriceMultiplied = item.Price;

                    ItemList.Add(item);
                    i++;
                    itemAvailability = "";

                    //
                    totalAmount = totalAmount + item.Price;
                    //

                    return RedirectToAction("Index");

                }

                else
                {
                    itemAvailability = "Item not available";
                    return RedirectToAction("Index");

                }
            }

        }

        public ActionResult FinishBilling()
        {

            //XmlSerializer xs = new XmlSerializer(typeof(List<Item>));

            //TextWriter txtWriter = new StreamWriter(@"C:\Users\jn185117\Desktop\sample1.xml");

            //xs.Serialize(txtWriter, ItemList);

            //txtWriter.Close();
            HttpClient client = new HttpClient();
            Uri baseAddress = new Uri("http://localhost:53297/");
            client.BaseAddress = baseAddress;
            HttpResponseMessage response = client.PostAsJsonAsync("api/Items?TotalAmount=" + totalAmount, ItemList).Result;
            string op = "";
           
            if (response.IsSuccessStatusCode)
            {
                using (HttpContent cont = response.Content)
                {
                    Task<string> res = cont.ReadAsStringAsync();
                    op = res.Result;
                                     }

                ViewBag.data = totalAmount;
                ViewBag.id = op;
                
                return View(ItemList);
            }

            else
            {
                itemAvailability = "Last transaction not saved";
                return RedirectToAction("Index");
            }


           
        }

        public ActionResult NewTransaction()
        {
            ItemList.Clear();
            itemAvailability = "";
            totalAmount = 0;
            return RedirectToAction("Index");

        }

    }
}