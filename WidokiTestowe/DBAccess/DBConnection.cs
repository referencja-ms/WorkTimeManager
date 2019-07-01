using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WorkTimeManager.DBAccess;

namespace WorkTimeManager.DBAccess
{
    class DBConnection
    {
        private static DBConnection instance = null;
        public MySqlConnection Connection { get; private set; }
        public static DBConnection Instance
        { //singleton
            get
            {
                //if null pierwsze, else drugie
                return instance ?? (instance = new DBConnection());
            }
        }
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
