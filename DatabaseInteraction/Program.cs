/* References
 * 
 * SQL queries https://stackoverflow.com/questions/18754688/c-sharp-how-to-implement-method-that-return-list-of-sql-result
 * SQL queries on MSDN https://msdn.microsoft.com/en-us/library/fksx3b4f.aspx
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

        public class dbAction
        {
            private static string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=localtest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            public static List<Person> listusers()
            {


                SqlConnection sqlConnection1 = new SqlConnection(ConnectionString);
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

                return listOfPerson;

            }
            public static void exit()
            {
                Environment.Exit(0);
            }
            public static void adduser()
            {
                //first get the user input and build out the Person object
                Person newPerson = new Person();
                Console.Write("ID: ");
                int userInputID; //this is all just to parse out the int.
                var temp = Int32.TryParse(Console.ReadLine(), out userInputID);
                newPerson.id = userInputID;
                //Int32.TryParse(useraction, out numericResult)

                Console.Write("Username: ");
                newPerson.username = Console.ReadLine();
                Console.Write("Password: ");
                newPerson.password = Console.ReadLine();
                Console.Write("Email: ");
                newPerson.email = Console.ReadLine();


                SqlConnection sqlConnection1 = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = String.Format("INSERT INTO Users(id, username, password, email) values ({0},'{1}','{2}','{3}')", newPerson.id, newPerson.username, newPerson.email, newPerson.password);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();

            }

            public static void deleteuser()
            {
                Console.Write("Delete which ID?: ");
                //collect user ID and parse it into an int.
                int userInputID; //this is all just to parse out the int.
                var temp = Int32.TryParse(Console.ReadLine(), out userInputID); //the value I want is actually that guy on the end, userInputID

                SqlConnection sqlConnection1 = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = String.Format("DELETE FROM Users WHERE id = {0}", userInputID);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;

                sqlConnection1.Open();
                cmd.ExecuteNonQuery();
                sqlConnection1.Close();
                Console.Write(String.Format("Sucessfully deleted ID {0}", userInputID));
            }

        }
        


        static void Main(string[] args)
        {
            /* Instructions:
             * l = list (done)
             * e = edit
             * a = add (done)
             * d = delete (done)
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
                //if (useraction == 'x'.ToString() || useraction == 'q'.ToString()) {
                //    break;
                //}
                
                //see if we chose to clear screen
                if (useraction == 'c'.ToString())
                {
                    
                }


                switch (useraction)
                {
                    case "l":
                        dbAction.listusers();
                        break;
                    case "q":
                        dbAction.exit();
                        break;
                    case "x":
                        dbAction.exit();
                        break;
                    case "c":
                        Console.Clear();
                        break;
                    case "a":
                        dbAction.adduser();
                        break;
                    case "d":
                        dbAction.deleteuser();
                        break;


                    default:
                        break;
                }



                Console.WriteLine("\n");
            } while (true);
            
        }
    }
}
