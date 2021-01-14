using System;
using System.Collections.Generic;
using System.Text;

namespace HHP.Models.Page
{
    public enum MenuItemType
    {
        Home,
        Household,
        Purchase,
        Settings,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
