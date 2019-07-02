using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Models
{
    class User
    {
        public string Login { get; set; }
        public string Position { get; set; }
        public User(string login, string position = "")
        {
            Login = login;
            Position = position;
        }
    }
}
