using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace WorkTimeManager.Models {
    public class Project {
        private string _name;
		
		#region Properties
		public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Budget { get; set; }
        public int TimeBudget { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public string CustomerNIP { get; set; }
		#endregion
		
        public Project(IDataReader dataReader) {
            _name = dataReader["name"].ToString();
        }
		
		public Project (MySqlDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"]);
            Name = reader["name"].ToString();
            Status = reader["status"].ToString();
            Budget = Convert.ToInt32(reader["budget"]);
            TimeBudget = Convert.ToInt32(reader["timeBudget"]);
            Deadline = (DateTime)reader["deadline"];
            Description = reader["description"].ToString();
            CustomerNIP = reader["customerNIP"].ToString();
        }
		
		public Project(string name, string status, int budget, int timeBudget, DateTime deadline, string description, string customerNIP)
        {
            Name = name;
            Status = status;
            Budget = budget;
            TimeBudget = timeBudget;
            Deadline = deadline;
            Description = description;
            CustomerNIP = customerNIP;
        }
		
        public override string ToString() {
            return $"{_name}";
        }
    }
}
