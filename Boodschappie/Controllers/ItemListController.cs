using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boodschappie.Models;
using Boodschappie.Filters;

namespace Boodschappie.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ItemListController : Controller
    {
        private AppContext db = new AppContext();

        //
        // GET: /ItemList/

        public ActionResult Index()
        {
            return View(db.ItemList.ToList());
        }

        //
        public ActionResult BlankItemRow()
        {

            return PartialView("ItemsRowPartial", new Items());
        }

        //
        // GET: /ItemList/Details/5

        public ActionResult Details(long id = 0)
        {
            ItemList itemlist = db.ItemList.Find(id);
            if (itemlist == null)
            {
                return HttpNotFound();
            }
            return View(itemlist);
        }

        //
        // GET: /ItemList/Create

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

                    new Items { }
                }

            };

            return View(model);
        }

        //
        // POST: /ItemList/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemList itemlist)
        {
            if (ModelState.IsValid)
            {
               
                if (itemlist.sharedWith == null)
                {
                    itemlist.sharedWith = new List<UserProfile>();
                    itemlist.sharedWith.Add(UserProfile.getUser(itemlist.UserId));

                }

                if (ModelState.IsValid)
                {
                    db.ItemList.Add(itemlist);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(itemlist);

            }

            return View(itemlist);
        }

        //
        // GET: /ItemList/Edit/5

        public ActionResult Edit(long id = 0)
        {
            ItemList itemlist = db.ItemList.Find(id);

            itemlist.Items = Items.getItemsList(id);

            if (itemlist == null)
            {
                return HttpNotFound();
            }
            else if (itemlist.Items.Count() < 1) {
                return HttpNotFound();
            }

            return View(itemlist);
        }

        //
        // POST: /ItemList/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemList itemlist)
        {
            if (ModelState.IsValid)
            {
                
                List<Items> items = Items.getItemsList(itemlist.ItemListId);
                
                foreach (var item in itemlist.Items)
                {
                    if (items.Contains(item))
                    {
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else {

                        item.ItemListId = itemlist.ItemListId;

                        if (ModelState.IsValid) {
                            db.Items.Add(item);
                            db.SaveChanges();

                        }
                    
                    }
                    
                }

                db.Entry(itemlist).State = EntityState.Modified;
                db.SaveChanges();
               
                return RedirectToAction("Index");
            }
            return View(itemlist);
        }

        //
        // GET: /ItemList/Delete/5

        public ActionResult Delete(long id = 0)
        {
            ItemList itemlist = db.ItemList.Find(id);
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
            ItemList itemlist = db.ItemList.Find(id);
            db.ItemList.Remove(itemlist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}