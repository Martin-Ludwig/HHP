using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHP.Models
{
    public class User
    {

        [BsonId]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string HouseholdId { get; set; }
        public string Password { get; set; }


        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
