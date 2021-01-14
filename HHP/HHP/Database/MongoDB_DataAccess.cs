using HHP.secrets;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHP.Database
{

    class MongoDB_DataAccess
    {
        /// <summary>
        /// MongoClient object.
        /// </summary>
        private MongoClient _client;

        /// <summary>
        /// MongoDB database object.
        /// </summary>
        private IMongoDatabase _database;

        /// <summary>
        /// Name of the database.
        /// </summary>
        private string Database
        {
            get;
        }

        /// <summary>
        /// Connects to a MongoDB database.
        /// </summary>
        public MongoDB_DataAccess()
        {
            this.Database = DB_con.Database;

            InitializeConnection();
        }


        /// <summary>
        /// Builds the connection to the database.
        /// </summary>
        private void InitializeConnection()
        {
            //string url = $"mongodb+srv://{DB_con.Username}:{DB_con.Passwort}@{DB_con.Url}";
            string url = DB_con.ConnectionString;

            _client = new MongoClient(url);
            _database = _client.GetDatabase(Database);
        }

        /// <summary>
        /// Inserts a new record into the collection.
        /// </summary>
        public void Insert<T>(string table, T record)
        {
            var collection = _database.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        /// <summary>
        /// Returns all entries from the collection.
        /// </summary>
        public List<T> FindAll<T>(string table)
        {
            var collection = _database.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        /// <summary>
        /// Returns all entries from the collection with a specific filter.
        /// </summary>
        public List<T> FindAll<T>(string table, FilterDefinition<T> filter)
        {
            var collection = _database.GetCollection<T>(table);
            return collection.Find(filter).ToList();
        }

        /// <summary>
        /// Returns object from database
        /// </summary>
        public T Find<T>(string table, BsonDocument bson)
        {
            var collection = _database.GetCollection<T>(table);

            var result = collection.Find(bson);

            if (result != null)
            {
                return result.FirstOrDefault();
            }

            return default(T);
        }

        /// <summary>
        /// Returns a Document by attribute and value
        /// </summary>
        public T Find<T>(string table, string attr, string value)
        {
            var collection = _database.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq(attr, value);

            var result = collection.Find(filter);

            if (result != null)
            {
                return result.FirstOrDefault();
            }

            // returns null
            return default(T);
        }

        /// <summary>
        /// Updates a document.
        /// Inserts new Documents on insertIfNotExists = true.
        /// </summary>
        public void Update<T>(string table, BsonDocument doc, T record, bool insertIfNotExists = true)
        {
            var collection = _database.GetCollection<T>(table);

            try
            {
                var result = collection.ReplaceOne(
                    doc,
                    record,
                    new ReplaceOptions { IsUpsert = insertIfNotExists });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        /// <summary>
        /// Deletes a record in the collection.
        /// </summary>
        public void Delete<T>(String table, Guid id)
        {
            var collection = _database.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }

        /// <summary>
        /// Deletes a record in the collection.
        /// </summary>
        public void Delete<T>(String table, BsonDocument bson)
        {
            var collection = _database.GetCollection<T>(table);
            collection.DeleteOne(bson);
        }

        /// <summary>
        /// Checks if document already exists.
        /// </summary>
        /// <returns>True if Document exists.</returns>
        public bool Exists<T>(String table, FilterDefinition<T> filter, FindOptions options = null)
        {
            var count = _database.GetCollection<T>(table).Find(filter, options).Limit(1).CountDocuments();

            return (count != 0);
        }


    }
}
