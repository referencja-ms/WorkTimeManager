using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WorkTimeManager.Models
{
    class UsersList
    {
        private string _name;
        public UsersList(IDataReader dataReader)
        {
            _name = dataReader["concat(firstname,\" \",lastname)"].ToString();
        }
        public override string ToString()
        {
            return $"{_name}";
        }
    }
}
