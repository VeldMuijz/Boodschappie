using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boodschappie.Models
{
    public  class DbInitializer : System.Data.Entity.DropCreateDatabaseAlways<AppContext>
    {
       //Custom initializer of the database. This will seed all the data that is in this class

        protected override void Seed(AppContext context)
        {
            
            var items = new List<Items>{
                new Items{ ItemName="Item1", Format="gram", Quantity=4},
                new Items{ ItemName="Item2", Format="gram", Quantity=4},
                new Items{ ItemName="Item3", Format="gram", Quantity=4},
                new Items{ ItemName="Item4", Format="gram", Quantity=4},
                new Items{ ItemName="Item5", Format="gram", Quantity=4},
                new Items{ ItemName="Item6", Format="gram", Quantity=4},
                new Items{ ItemName="Item7", Format="gram", Quantity=4},
                new Items{ ItemName="Item8", Format="gram", Quantity=4},
                new Items{ ItemName="Item9", Format="gram", Quantity=4}

            };

            var listitem = new List<ItemList>
            {

                new ItemList {ItemListName="Seedlist1", Items = new List<Items> { items[1]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist2", Items = new List<Items> { items[2]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist3", Items = new List<Items> { items[3]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist4", Items = new List<Items> { items[4]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist5", Items = new List<Items> { items[5]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist6", Items = new List<Items> { items[6]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist7", Items = new List<Items> { items[7]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist8", Items = new List<Items> { items[8]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist9", Items = new List<Items> { items[8]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist10", Items = new List<Items> { items[1]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist11", Items = new List<Items> { items[2]}, LastUpdate = DateTime.Now, UserId=1},
                new ItemList {ItemListName="Seedlist12", Items = new List<Items> { items[3]}, LastUpdate = DateTime.Now, UserId=1}

            };
            System.Diagnostics.Debug.WriteLine("[DB: Initializing table Itemlist]");
            listitem.ForEach(i => context.ItemList.Add(i));
            context.SaveChanges();

            System.Diagnostics.Debug.WriteLine("[DB: Initializing table SharedWith]");
            var sharedwith = new List<SharedWith>
            {
                new SharedWith { ItemListId=1, UserId=1},
                new SharedWith { ItemListId=2, UserId=1},
                new SharedWith { ItemListId=2, UserId=2},
                new SharedWith { ItemListId=3, UserId=1},
                new SharedWith { ItemListId=4, UserId=1},
                new SharedWith { ItemListId=4, UserId=2},
                new SharedWith { ItemListId=4, UserId=3},
            };
            sharedwith.ForEach(i => context.SharedWith.Add(i));
            context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("[DB: Execute Seed]");
            base.Seed(context);
        }


    }
}