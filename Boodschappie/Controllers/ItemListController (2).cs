using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boodschappie.Models;
using System.Web.Security;
using Boodschappie.Filters;


namespace Boodschappie.Controllers
{

    
    public class ItemListController : Controller
    {
        private AppContext dbContext = new AppContext();
        

        //
        // GET: /ItemList/

        public ActionResult Index()
        {
            return View(dbContext.ItemList.ToList());
        }

        //
        // GET: /ItemList/Details/5

        public ActionResult Details(long id = 0)
        {
            ItemList itemlist = dbContext.ItemList.Find(id);
            if (itemlist == null)
            {
                return HttpNotFound();
            }
            return View(itemlist);
        }

        //
        // GET: /ItemList/Create
        [InitializeSimpleMembership]
        public ActionResult Create()
        {
           var model = new ItemList
            {

            UserId = WebMatrix.WebData.WebSecurity.GetUserId(User.Identity.Name),
            ItemListName = "",
            LastUpdate = DateTime.Now,
            sharedWith = new List<UserProfile>{
                UserProfile.getUser(WebMatrix.WebData.WebSecurity.GetUserId(User.Identity.Name))
            },
            Items = new List<Items> { 

                    new Items { ItemName = "", Quantity = 0, Format = ""}
            }


            };

            return View(model);
        }

        public ActionResult BlankItemRow()
        {
            return PartialView("ItemsRowPartial", new Items());
        }

        //
        // POST: /ItemList/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemList itemlist)
        {
            if (itemlist.sharedWith == null) {
                itemlist.sharedWith.Add(UserProfile.getUser(itemlist.UserId));

            }
            if (ModelState.IsValid)
            {
                
                    dbContext.ItemList.Add(itemlist);
                    dbContext.SaveChanges();


                
                return RedirectToAction("Index");
            }

            return View(itemlist);
        }

        //
        // GET: /ItemList/Edit/5

        public ActionResult Edit(long id)
        {
            ItemList itemlist = dbContext.ItemList.Find(id);

            if (itemlist == null)
            {
                return HttpNotFound();
            }


            if (itemlist.checkUser(WebMatrix.WebData.WebSecurity.GetUserId(User.Identity.Name), itemlist.ItemListId))
            {
                var query = (from a in dbContext.Items
                             where a.ItemListId.Equals(id)
                             select a).ToList();

                itemlist.Items = query;

                return View(itemlist);
            }

            // var query = (from a in dbContext.Items
            //                 where a.listID.Equals(id)
            //                 select a).ToList();

            //itemlist.Items = query;

            //return View(itemlist);
            
            return HttpNotFound();
        }

        //
        // POST: /ItemList/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemList itemlist)
        {
            

            if (ModelState.IsValid)
            {

                dbContext.Entry(itemlist).State = EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(itemlist);
        }

        //
        // GET: /ItemList/Delete/5

        public ActionResult Delete(long id = 0)
        {
            ItemList itemlist = dbContext.ItemList.Find(id);
            if (itemlist == null)
            {
                return HttpNotFound();
            }
            return View(itemlist);
        }

        //
        // POST: /ItemList/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ItemList itemlist = dbContext.ItemList.Find(id);
            dbContext.ItemList.Remove(itemlist);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}