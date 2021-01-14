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
    /// <para>db.User.FindAll();</para>
    /// </code>
    /// </summary>
    class UserDB
    {

        private static MongoDB_DataAccess _db;
        private static string _collection;

        public UserDB(MongoDB_DataAccess database, string collection)
        {
            _db = database;
            _collection = collection;
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        public void Add(User user)
        {
            _db.Insert(_collection, user);
        }

        /// <summary>
        /// Adds a new user without duplicates.
        /// </summary>
        public void AddIfNotExists(User user)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq("Name", user.Name);

            if (!_db.Exists<User>(_collection, filter))
            {
                _db.Insert(_collection, user);
            }
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns>List of Users</returns>
        public List<User> FindAll()
        {
            return _db.FindAll<User>(_collection);
        }

        /// <summary>
        /// Finds all user from a household.
        /// </summary>
        /// <param name="householdId">Household-Id</param>
        /// <returns>Returns a list of users that are a member of the household.</returns>
        public List<User> FindAllUserFromHousehold(String householdId)
        {
            List<User> users = new List<User>();

            // filter
            var builder = Builders<User>.Filter;
            var filter = builder.Eq("HouseholdId", householdId);


            return _db.FindAll<User>(_collection, filter);
        }

        /// <summary>
        /// Returns the first user that matches the user object.
        /// </summary>
        public User Find(User user)
        {
            return _db.Find<User>(_collection, user.ToBsonDocument());
        }

        /// <summary>
        /// Returns the first user that matches the name.
        /// </summary>
        public User FindByName(string name)
        {
            return _db.Find<User>(_collection, "Name", name);
        }

        /// <summary>
        /// Returns a user if id matches.
        /// </summary>
        public User FindById(Guid id)
        {
            return _db.Find<User>(_collection, "Id", id.ToString());
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        public void Delete(Guid id)
        {
            _db.Delete<User>(_collection, id);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        public void Delete(User user)
        {
            _db.Delete<User>(_collection, user.ToBsonDocument());
        }


        /// <summary>
        /// Updates a user.
        /// </summary>
        public void Update(User user)
        {
            _db.Update<User>(_collection, new BsonDocument("Name", user.Name), user);
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        public void Update(User oldUser, User newUser)
        {
            _db.Update<User>(_collection, oldUser.ToBsonDocument(), newUser);
        }

        /// <summary>
        /// Updates a user that matches the id.
        /// </summary>
        public void UpdateById(Guid id, User newUser)
        {
            _db.Update<User>(_collection, new BsonDocument("_id", id), newUser);
        }

        /// <summary>
        /// Updates a user that matches the name.
        /// </summary>
        public void UpdateByName(string name, User newUser)
        {
            _db.Update<User>(_collection, new BsonDocument("Name", name), newUser);
        }

        /// <summary>
        /// Checks if a user already exists.
        /// </summary>
        public bool Exists(User user)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq("Name", user.Name);

            return _db.Exists<User>(_collection, filter);
        }

    }
}
