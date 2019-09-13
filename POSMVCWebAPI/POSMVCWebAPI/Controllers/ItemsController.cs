using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Linq;
using POSMVCWebAPI;
using POSMVCWebAPI.Models;

namespace POSMVCWebAPI.Controllers
{
    public class ItemsController : ApiController
    {
        private POSEntities db = new POSEntities();
        private TransactionsEntities TransactionDb = new TransactionsEntities();

        // GET: api/Items
        public IQueryable<Item> GetItems()
        {
            return db.Items;
        }

        // GET: api/Items/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult GetItem(string id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutItem(string id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.ItemId)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Items
        //[ResponseType(typeof(Item))]
        //public IHttpActionResult PostItem(Item item)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Items.Add(item);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (ItemExists(item.ItemId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = item.ItemId }, item);
        //}

       [HttpPost]
        public IHttpActionResult PostItem(List<TransactionList> transactionList,[FromUri] decimal TotalAmount)
        {
           if(transactionList.Count == 0)
            {
                return BadRequest();
            }

            else
            {

                var obj = (from m in TransactionDb.Transactions
                           orderby m.TransactionId descending
                           select m).FirstOrDefault();

                int LastTransactionId = obj.TransactionId;

                LastTransactionId++;
                var xEle = new XElement("Transaction",
                    new XAttribute("TransactionId", LastTransactionId),
                    new XAttribute("TotalAmount", TotalAmount),
                from transaction in transactionList
                select new XElement("Item",
                             
                               new XElement("ItemID", transaction.ItemId),
                               new XElement("ItemName", transaction.Name),
                               new XElement("Quantity", transaction.Quantity),
                               new XElement("UnitPrice", transaction.Price),
                               new XElement("Amount", transaction.PriceMultiplied)

                           ));

               

                SqlConnection con = new SqlConnection(@"Data Source=WINJN185117-Y81\SQLEXPRESS;Initial Catalog=POS;Integrated Security=True");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                
                cmd.Parameters.Add(new SqlParameter("@TransactionId", SqlDbType.Int));
                cmd.Parameters["@TransactionId"].Value = LastTransactionId;
                cmd.Parameters.Add(new SqlParameter("@TransactionXml", SqlDbType.Xml));
                cmd.Parameters["@TransactionXml"].Value = xEle.ToString();
                cmd.Parameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));
                cmd.Parameters["@TransactionDate"].Value = DateTime.Now;

                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    "INSERT INTO Transactions (TransactionId, TransactionList, TransactionDate) " +
                    "VALUES (@TransactionId, @TransactionXml, @TransactionDate)";

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


                return Ok(LastTransactionId);
            }
        }

        // DELETE: api/Items/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult DeleteItem(string id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            db.SaveChanges();

            return Ok(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(string id)
        {
            return db.Items.Count(e => e.ItemId == id) > 0;
        }
    }
}