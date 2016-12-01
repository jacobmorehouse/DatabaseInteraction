/* References
 * 
 * SQL queries https://stackoverflow.com/questions/18754688/c-sharp-how-to-implement-method-that-return-list-of-sql-result
 * 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    class Program
    {
        public class Person
        {
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string email { get; set; }
        }


        static void Main(string[] args)
        {
            /* Instructions:
             * l = list
             * e = edit
             * a = add
             * d = delete
             * x / q = exit (done)
             * c = clear (done)
             * 
             * Everything is identified by ID when taking an action on it.
             * 
             */

            string myConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=localtest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            do
            {
                //get user input
                Console.WriteLine("What would you like to do?");
                string useraction = Console.ReadLine();

                //verify length = 1
                if (useraction.Length != 1)
                {
                    Console.WriteLine("input should be 1 char!");
                }

                //check if numeric
                int numericResult;
                var isItNumeric = Int32.TryParse(useraction, out numericResult);
                if (isItNumeric == true)
                {
                    Console.WriteLine("input should not be integers!");
                }

                //see if we chose to exit
                if (useraction == 'x'.ToString() || useraction == 'q'.ToString()) {
                    break;
                }
                
                //see if we chose to clear screen
                if (useraction == 'c'.ToString())
                {
                    Console.Clear();
                }


                //List users
                if (useraction == 'l'.ToString())
                {
                    SqlConnection sqlConnection1 = new SqlConnection(myConnectionString);
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;

                    cmd.CommandText = "SELECT id, username, password, email FROM Users";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;

                    sqlConnection1.Open();

                    reader = cmd.ExecuteReader();
                    // Data is accessible through the DataReader object.

                    //first instantiate a generic list to store the Person objects
                    var listOfPerson = new List<Person>();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //build our new person object for each row
                            var person = new Person();
                            person.username = reader["username"].ToString();
                            person.password = reader["password"].ToString();
                            person.id = Convert.ToInt32(reader["id"]);
                            person.email = reader["email"].ToString();

                            //add the person to the list
                            listOfPerson.Add(person);
                        }
                    }
                    else 
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();

                    sqlConnection1.Close();

                    Console.WriteLine("\nList of users:");
                    foreach (var p in listOfPerson)
                    {
                        Console.WriteLine(p.id + ", " + p.username + ", " + p.password + ", " + p.email);
                    }

                }

                Console.WriteLine("\n");
            } while (true);
            
            //commit test2
        }
    }
}
