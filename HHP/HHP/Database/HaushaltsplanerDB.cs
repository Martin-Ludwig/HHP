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
    /// Access to the database.
    /// <code>var db = new HaushaltsplanerDB();</code>
    /// </summary>
    class HaushaltsplanerDB
    {
        private static MongoDB_DataAccess _db = new MongoDB_DataAccess();
        private const string UserCollection = "Users";
        private const string HousehouldCollection = "Households";

        private static UserDB _userdb = new UserDB(_db, UserCollection);
        private static HouseholdDB _householddb = new HouseholdDB(_db, HousehouldCollection);

        /// <summary>
        /// Access to the User collection.
        /// </summary>
        public UserDB User => _userdb;

        /// <summary>
        /// Access to the Household collection.
        /// </summary>
        public HouseholdDB Household => _householddb;


        public HaushaltsplanerDB()
        {

        }

    }
}
