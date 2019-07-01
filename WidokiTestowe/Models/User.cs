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
        public User(string login)
        {
            Login = login;
        }
    }
}
