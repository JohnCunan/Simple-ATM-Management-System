using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AtmApplication
{
    public class DataAccess
    {
        //Connection string for database
        public static string conString = "Data Source=John-Acer\\SQLEXPRESS;Initial Catalog=CRUDApps;Integrated Security=True";

        //Read function from CRUD
        //Gets the current account number logged in
        public static int getAccountNumber(string accNumFromText)
        {
            int  retrievedAcc = 0;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                string query = "SELECT AccountNumber FROM tblAccounts WHERE AccountNumber = @AccNum";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccNum", int.Parse(accNumFromText));
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    retrievedAcc = (int)reader.GetValue(0);
                }
                return retrievedAcc;
            }
        }

        //Read function from CRUD
        //Gets the current full name by using the account number logged in
        public static string getFullName(int currentAccNum)
        {
            string fName = " ", mName = " ", lName = " ";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                string query = "SELECT FirstName, MiddleName, LastName FROM tblAccounts WHERE AccountNumber = @AccNum";
                SqlCommand command = new SqlCommand(query, connection);

                //currentAccNum will be retrieved from the logged user
                //currentAccNum will be assigned to the parameter from the logged user account number
                command.Parameters.AddWithValue("@AccNum", currentAccNum);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    fName = reader.GetValue(0).ToString();
                    mName = reader.GetValue(1).ToString();
                    lName = reader.GetValue(2).ToString();
                }
                return fName + " " + mName + " " + lName;
            }
        }

        public static int getBalance(int currentAccNum)
        {
            int balance = 0;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                string query = "SELECT Balance FROM tblAccounts WHERE AccountNumber = @AccNum";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@AccNum", currentAccNum);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    balance = Convert.ToInt32(reader.GetValue(0));
                }
                return balance;
            }
        }
    }
}
