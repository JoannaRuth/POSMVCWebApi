using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using POSMVCWebAPI.Models;

namespace POSMVCWebAPI.Controllers
{
    public class AssociatesController : ApiController
    {
        private POSEntities2 db = new POSEntities2();

        // GET: api/Associates
        public IQueryable<Associate> GetAssociates()
        {
            return db.Associates;
        }

        // GET: api/Associates/5
        [ResponseType(typeof(Associate))]
        public IHttpActionResult GetAssociate(int id)
        {
            Associate associate = db.Associates.Find(id);
            if (associate == null)
            {
                return NotFound();
            }

            return Ok(associate);
        }

        // PUT: api/Associates/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssociate(int id, Associate associate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != associate.AssociateId)
            {
                return BadRequest();
            }

            db.Entry(associate).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssociateExists(id))
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


        // DELETE: api/Associates/5
        [ResponseType(typeof(Associate))]
        public IHttpActionResult DeleteAssociate(int id)
        {
            Associate associate = db.Associates.Find(id);
            if (associate == null)
            {
                return NotFound();
            }

            db.Associates.Remove(associate);
            db.SaveChanges();

            return Ok(associate);
        }

        // POST: api/Associates
        [ResponseType(typeof(Associate))]
        public IHttpActionResult PostAssociate(Associate associate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var associate1 = db.Associates.Where(a => a.AssociateId == associate.AssociateId && a.AssociatePwd.Equals(associate.AssociatePwd))
                                        .SingleOrDefault();
            if (null == associate1)
                return BadRequest(ModelState);
            else
                associate.RoleId = associate1.RoleId;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AssociateExists(associate.AssociateId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }



            return CreatedAtRoute("DefaultApi", new { id = associate1.AssociateId }, associate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssociateExists(int id)
        {
            return db.Associates.Count(e => e.AssociateId == id) > 0;
        }
    }
}