using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHP.Models
{
    public class Item
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int HouseholdId { get; set; }

        /// <summary>
        /// Increases Count.
        /// </summary>
        /// <param name="amount">Amount that will be increased. (Default by 1)</param>
        public void Add(int amount = 1)
        {
            Count += amount;
        }

        /// <summary>
        /// Decreases Count.
        /// </summary>
        /// <param name="amount">Amount that will be decreased. (Default by 1)</param>
        public void Remove(int amount = 1)
        {
            Count -= amount;
        }

        /// <summary>
        /// Checks if Count is zero.
        /// </summary>
        /// <returns>True if Count equals zero.</returns>
        public bool IsEmpty()
        {
            return (Count == 0);
        }

    }
}
