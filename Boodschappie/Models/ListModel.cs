using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boodschappie.Models
{

    [Table("ItemList")]
    public class ItemList
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ItemListId { get; set; }
        public long UserId { get; set; }
        [Required]
        public String ItemListName { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<SharedWith> SharedWith { get; set; }
        public List<Items> Items { get; set; }

    }

    [Table("SharedWith")]
    public class SharedWith {
        public long SharedWithId { get; set; }
        public long ItemListId { get; set; }
        public long UserId { get; set; }
        public String UserName { get; set; }

        private AppContext db = new AppContext();
        public Boolean checkUser(long userid, long listid)
        {
            var result = false;

            var up = UserProfile.getUser(userid);

            SharedWith sw = db.SharedWith.Find(listid);

            if (sw != null && up != null)
            {
                result = true;
            }

            return result;
        }

        public static List<SharedWith> GetListShared(long listId)
        {
            using (AppContext db = new AppContext())
            {
                List<SharedWith> listsharedWith = (from sharedWith in db.SharedWith
                                                   where sharedWith.ItemListId == listId
                                                   select sharedWith).ToList();
                return listsharedWith;
            }
        }

    }

    [Table("Items")]
    public class Items
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ItemsId { get; set; }
        [Required]
        public long ItemListId { get; set; }
        [Required]
        public String ItemName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public String Format { get; set; }

        private AppContext db = new AppContext();

        public static List<Items> getItemsList(long listId) {

            using (AppContext db = new AppContext()) {
                List<Items> listItems = (from items in db.Items
                                         where items.ItemListId == listId
                                         select items).ToList();
                return listItems;
            } 
            
        }
    
    }

}