using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WorkTimeManager
{
    public class Customer
    {
        #region Properties
        public string NIP { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        #endregion

        #region Constructors
        public Customer(MySqlDataReader dataReader)
        {
            NIP = dataReader["customerNIP"].ToString();
            Name = dataReader["name"].ToString();
            Email = dataReader["emailAddress"].ToString();
            PhoneNumber = dataReader["phoneNumber"].ToString();
            Address = dataReader["address"].ToString();
        }

        public Customer(string NIP, string Name, string Email, string PhoneNumber, string Address)
        {
            this.NIP = NIP;
            this.Name = Name;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.Address = Address;
        }
        #endregion

        public override string ToString()
        {
            return $"{NIP} {Name} {Email} {PhoneNumber} {Address}";
        }
    }
}
