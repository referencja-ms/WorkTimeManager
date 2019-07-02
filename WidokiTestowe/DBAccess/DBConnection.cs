using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WorkTimeManager.DBAccess
{
    class DBConnection
    {
        private static DBConnection instance = null;
        public MySqlConnection Connection { get; private set; }
        public static Models.User LoggedUser { get; set; }
        public static DBConnection Instance
        { //singleton
            get
            {
                return instance ?? (instance = new DBConnection());
            }
        }
        //Utworzenie połączenia z bazą
        private DBConnection()
        {
            MySqlConnectionStringBuilder conStringBuilder = new MySqlConnectionStringBuilder();
            conStringBuilder.Port = uint.Parse(DBInfo.port);
            conStringBuilder.UserID = DBInfo.user;
            conStringBuilder.Database = DBInfo.database;
            conStringBuilder.Password = DBInfo.password;
            conStringBuilder.Server = DBInfo.server;
            Console.WriteLine(conStringBuilder.ToString());
            Connection = new MySqlConnection(conStringBuilder.ToString());
        }
    }
}
