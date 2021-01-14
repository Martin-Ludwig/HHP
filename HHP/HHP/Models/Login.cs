using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHP.Database;
using HHP.Utils;

namespace HHP.Models
{
    public class Login
    {
        private string Username { get; set; }
        private string HashedPwd { get; set; }
        public static bool CheckIfUserExists(string username)
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            if (db.User.FindByName(username) != null)
                return true;
            else
                return false;
        }
        public static void RegisterJoinHousehold(string username, string password, string householdID)
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            string hashed_pwd = Crypto.ComputeSHA256(password);
            db.User.AddIfNotExists(new User() { Name = username, Password = hashed_pwd, HouseholdId = householdID});

        }
        public static void RegisterNewHousehold(string username, string password, string householdName)
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            string hashed_pwd = Crypto.ComputeSHA256(password);
            db.Household.AddIfNotExists(new Household() { Name = householdName});
            //db.User.AddIfNotExists(new User() { Name = username, Password = hashed_pwd, HouseholdId = household.HouseholdId}); //TODO HouseholdId

        }
        public static bool LoginUser(string username, string password)
        {
            HaushaltsplanerDB db = new HaushaltsplanerDB();
            string hashed_pwd = Crypto.ComputeSHA256(password);
            User user = db.User.FindByName(username);
            Household household = db.Household.FindByHouseholdId(user.HouseholdId);
            if (user.Password == hashed_pwd)
            {
                Session Session = new Session(user, household);
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
