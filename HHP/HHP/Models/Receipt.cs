using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHP.Models
{
    public class Receipt
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public Guid Id { get; set; }
        public string ImageUrl;
    }
}
