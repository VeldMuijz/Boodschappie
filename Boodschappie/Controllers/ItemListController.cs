﻿using System;
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
        // GET: /ItemList/BlankItemRow()
        public ActionResult BlankItemRow()
        {

            return PartialView("ItemsRowPartial", new Items());
        }

        //
        // GET: /ItemList/SharedWith
        public ActionResult BlankSharedWithRow()
        {

            return PartialView("SharedWithPartial", new SharedWith());


        }

        //
        // GET: /ItemList/AddSharedWith/4
        public ActionResult AddSharedWith(long id)
        {
            var user = UserProfile.getUser(id);

            return PartialView("SharedWithPartial", new SharedWith { UserId = user.UserId, UserName = user.UserName });

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
                SharedWith = new List<SharedWith> {
                    new SharedWith{ }

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

                if (itemlist.SharedWith != null)
                {
                    List<SharedWith> sw = new List<SharedWith>();

                    foreach (var user in itemlist.SharedWith)
                    {
                        var userdetails = UserProfile.getUser(user.UserId);

                        sw.Add(new SharedWith
                         {
                             ItemListId = itemlist.ItemListId,
                             UserId = userdetails.UserId,
                             UserName = userdetails.UserName
                         });

                    }

                    sw.Add(new SharedWith
                    {
                        ItemListId = itemlist.ItemListId,
                        UserId = WebMatrix.WebData.WebSecurity.CurrentUserId,
                        UserName = WebMatrix.WebData.WebSecurity.CurrentUserName
                    });

                    itemlist.SharedWith = sw;
                }
          

                itemlist.LastUpdate = DateTime.Now;


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
            itemlist.SharedWith = SharedWith.GetListShared(id);


            if (itemlist == null)
            {
                return HttpNotFound();
            }
            else if (itemlist.Items.Count() < 1)
            {
                return HttpNotFound();
            }



            return View(itemlist);
        }

        //Editing a itemlist, this includes CRUD on the items in the list
        // POST: /ItemList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemList itemlist)
        {
            if (ModelState.IsValid)
            {
                itemlist.LastUpdate = DateTime.Now;
                using (AppContext db = new AppContext())
                {
                    List<Items> items = Items.getItemsList(itemlist.ItemListId);

                    foreach (var item in itemlist.Items)
                    {
                        //If item is in database, update
                        if (items.Exists(x => x.ItemsId == item.ItemsId))
                        {
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                            db.Entry(item).State = EntityState.Detached;

                        }
                        else
                        {
                            //If item not in databaase, add/create
                            item.ItemListId = itemlist.ItemListId;

                            if (ModelState.IsValid)
                            {
                                db.Items.Add(item);
                                db.SaveChanges();

                            }

                        }

                    }

                    //Remove all removed items from db
                    foreach (var dbitem in items)
                    {
                        //For all items in db but no more in list, remove
                        if (!itemlist.Items.Exists(x => x.ItemsId == dbitem.ItemsId))
                        {
                            db.Items.Attach(dbitem);
                            db.Items.Remove(dbitem);
                            db.SaveChanges();

                        }

                        //save ItemList to db
                        db.Entry(itemlist).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }

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