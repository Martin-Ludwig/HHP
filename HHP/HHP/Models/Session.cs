using System;
using System.Collections.Generic;
using System.Text;
using HHP.Database;
using HHP.Models;
using Windows.System;

namespace HHP.Models
{
    public sealed class Session
    {
        public static User User { get; set; }
        public static Household Household { get; set; }

        /// <summary>
        /// List of all Users in the same Household
        /// </summary>
        public static List<User> HouseholdUsers { get; set; }
        public Session() { }
        public Session(User user, Household household)
        {
            User = user;
            Household = household;
            HouseholdUsers = FindAllHouseholdUsers();
        }

        private List<User> FindAllHouseholdUsers()
        {
            if (Session.Household != null && Session.Household.HouseholdId != null)
            {
                HaushaltsplanerDB db = new HaushaltsplanerDB();
                List<User> household_users = db.User.FindAllUserFromHousehold(Session.Household.HouseholdId);

                return household_users;
            }

            return new List<User>();
        }

        /// <summary>
        /// Overwrites session with data from database
        /// </summary>
        public void Pull()
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            User = db.User.FindByName(Session.User.Name);
            Household = db.Household.FindByHouseholdId(Session.Household.HouseholdId);
            HouseholdUsers = FindAllHouseholdUsers();
        }
    }
}
