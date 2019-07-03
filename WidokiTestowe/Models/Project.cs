using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WorkTimeManager.Models {
    class Project {
        private string _name;
        public Project(IDataReader dataReader) {
            _name = dataReader["name"].ToString();
        }
        public override string ToString() {
            return $"{_name}";
        }
    }
}
