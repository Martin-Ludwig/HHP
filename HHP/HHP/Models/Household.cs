using HHP.Models.Page;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHP.Models
{
    public class Household
    {
        List<Purchase> _purchases = new List<Purchase>();
        List<Item> _items = new List<Item>();

        [BsonId]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public String HouseholdId { get; set; }
        public List<Item> Items { get { return _items; } set { _items = value; } }
        public List<Purchase> Purchases { get { return _purchases; } set { _purchases = value; } }


        /// <summary>
        /// Adds a purchase to the purchase list.
        /// </summary>
        public void AddPurchase(Purchase p)
        {
            _purchases.Add(p);
        }

        /// <summary>
        /// Adds an item to the item list.
        /// </summary>
        public void AddItem(Item i)
        {
            _items.Add(i);
        }

        /// <summary>
        /// Removes the first occurrence of the specific item from the item list.
        /// </summary>
        public void DeleteItem(Item i)
        {
            _items.Remove(i);
        }

    }
}
