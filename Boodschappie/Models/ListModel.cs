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
        public virtual List<UserProfile> sharedWith { get; set; }
        public List<Items> Items { get; set; }
        
        
        
        private AppContext dbContext = new AppContext();

        public Boolean checkUser(long userid, long listid) {            
            var result = false;

            var up = UserProfile.getUser(userid);

            ItemList itemlist = dbContext.ItemList.Find(listid);

            if (itemlist != null && up != null)
            {
                result = itemlist.sharedWith.Contains(up);
            }

            return result;
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
    }

}