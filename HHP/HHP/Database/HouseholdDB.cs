using HHP.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHP.Database
{

    /// <summary>
    /// <para>
    /// Don't use this class. Use HaushaltsplanerDB instead.
    /// </para>
    /// Usage: 
    /// <code>
    /// <para>var db = new HaushaltsplanerDB();</para>
    /// <para>db.HouseholdDB.FindAll();</para>
    /// </code>
    /// </summary>
    class HouseholdDB
    {

        private static MongoDB_DataAccess _db;
        private static string _collection;

        public HouseholdDB(MongoDB_DataAccess database, string collection)
        {
            _db = database;
            _collection = collection;
        }

        /// <summary>
        /// Adds a new household.
        /// </summary>
        public void Add(Household household)
        {
            _db.Insert(_collection, household);
        }

        /// <summary>
        /// Adds a new household without duplicates.
        /// </summary>
        public void AddIfNotExists(Household household)
        {
            var builder = Builders<Household>.Filter;
            var filter = builder.Eq("HouseholdId", household.HouseholdId);

            if (!_db.Exists<Household>(_collection, filter))
            {
                _db.Insert(_collection, household);
            }
        }

        /// <summary>
        /// Returns all households.
        /// </summary>
        /// <returns>List of Users</returns>
        public List<Household> FindAll()
        {
            return _db.FindAll<Household>(_collection);
        }

        /// <summary>
        /// Looks for the exact same household.
        /// </summary>
        public Household Find(Household household)
        {
            return _db.Find<Household>(_collection, household.ToBsonDocument());
        }

        /// <summary>
        /// Returns the first household that matches the HouseholdId.
        /// </summary>
        public Household FindByHouseholdId(string householdId)
        {
            return _db.Find<Household>(_collection, "HouseholdId", householdId);
        }

        /// <summary>
        /// Deletes a household.
        /// </summary>
        public void Delete(Guid id)
        {
            _db.Delete<Household>(_collection, id);
        }

        /// <summary>
        /// Updates a household that matches the household id.
        /// </summary>
        public void Update(Household household)
        {
            if (household != null)
            {
                _db.Update<Household>(_collection, new BsonDocument("HouseholdId", household.HouseholdId), household);
            }
        }


        /// <summary>
        /// Updates a household that matches the household id.
        /// </summary>
        public void Update(string householdId, Household newHousehold)
        {
            _db.Update<Household>(_collection, new BsonDocument("HouseholdId", householdId), newHousehold);
        }

        /// <summary>
        /// Updates a household that matches the name.
        /// </summary>
        public void UpdateByName(string name, Household newHousehold)
        {
            _db.Update<Household>(_collection, new BsonDocument("Name", name), newHousehold);
        }

        /// <summary>
        /// Checks if a household already exists.
        /// </summary>
        public bool Exists(Household household)
        {
            var builder = Builders<Household>.Filter;
            var filter = builder.Eq("HouseholdId", household.HouseholdId);

            return _db.Exists<Household>(_collection, filter);
        }

        public void AddPurchse(Purchase p, String householdId)
        {
            Household h = this.FindByHouseholdId(householdId);
            h.AddPurchase(p);

            this.Update(householdId, h);
        }

    }
}
