using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Models {
    class Request {
        public string Login1 { get; set; }
        public string Login2 { get; set; }
        public string Project { get; set; }
        public Request(IDataReader dr) {
            Login1 = dr["senderlogin"].ToString();
            Login2 = dr["requestedlogin"].ToString();
            Project = dr["projectid"].ToString();
        }
    }
}
