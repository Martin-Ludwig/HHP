using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHP.Models
{
    public class Purchase
    {
        private String _title;
        private DateTime _date;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value.Length > 100)
                {
                    _title = value.Substring(0, 99);
                }
                else
                {
                    _title = value;
                }
            }
        }

        [BsonId]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }
        public double Price { get; set; }
        public DateTime Date { 
            get { return _date.Date; } 
            set { _date = value; }
        }

        /// <summary>
        /// Name of user that paid for the purchase.
        /// </summary>
        public string UserPayed { get; set; }

        /// <summary>
        /// List of users that are involved in this purchase.
        /// </summary>
        public List<string> UsersInvolved { get; set; }

        public List<string> UsersInvolvedPayed { get; set; }

        /// <summary>
        /// HouseholdId that belongs to the purchase.
        /// </summary>
        public int HouseholdId { get; set; }

        /// <summary>
        /// Receipt (Image)
        /// </summary>
        public Receipt Receipt { get; set; }


        public override string ToString()
        {
            return $"{Title} - {Date.ToString("dd.MM.yyyy")}";
        }
    }
}
