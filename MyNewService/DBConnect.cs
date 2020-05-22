using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNewService
{
    public class DBConnect
    {
        private SqlConnection cnn;
        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connetionString;
            
            connetionString = @"Data Source=LAPTOP-JL8U4Q2K;Initial Catalog=TestDB;User ID=sa;Password=Drafter0.3";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            cnn.Close();

        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                cnn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                cnn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Insert()
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "INSERT INTO name_table (id, name) VALUES('6', 'drafterr')";
            string txtTest = "";

            //open connection
            if (this.OpenConnection() == true)
            {
                txtTest = "connected";
                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = new SqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();

                command.Dispose();
                cnn.Close();
            }
            else {
                txtTest = "can't connect";
            }
            return txtTest;
        }
    }
}
