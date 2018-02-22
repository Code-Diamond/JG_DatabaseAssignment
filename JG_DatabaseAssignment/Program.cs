using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace JG_DBAssignment
{
    class Program
    {
        // Global Variables
        public static bool keepGoing = true;

        // Create new SqlConnection Object
        public static SqlConnection sql = new SqlConnection("Data Source=ROOT-PC\\SQLEXPRESS;Initial Catalog=classroom;Integrated Security=True");

        public static void Main(string[] args)
        {
            // Variables for the Main's scope

            // Initially created by Jamin Ghata  2/8/2018 
            Console.WriteLine("Welcome to SSMS_DB_Application!\n\n");

            // Loop
            start:
            do
            {
                // Clear the console each time the loop happens
                Console.Clear();

                // Display Menu
                displayMenu();

                // Retrieve Input
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine().ToUpper();
                Console.Clear();

                // If the user types in spaces 
                if (string.IsNullOrWhiteSpace(input))
                {
                    // Inform User the Entry Cannot be blank
                    Console.WriteLine("Entry Cannot be blank.");
                    Console.ReadKey();//Forces user to type something before clearing console

                    // Return user to start label
                    goto start;
                }
                // If the input is input other than spaces
                else
                {
                    mainMenu(input);
                }

                //promptToContinue();

            } while (keepGoing);



        }

        private static void ReadSingleRow(IDataRecord record)
        {
            Console.WriteLine(String.Format("{0}, {1}", record[0], record[1]));
        }

        //Main Menu functionality
        public static void mainMenu(string input)
        {
            switch (input)
            {
                // Create/Open a connection
                case "A":
                    //Catch Exception
                    try
                    {
                        //Attempt to open connection
                        Console.WriteLine("Opening the connection...");
                        sql.Open();
                        //Dictate to the user if the connection is set to open.
                        Console.WriteLine("The connection is currently set to {0}.", sql.State);
                        if (sql.State.ToString() == "Open")
                        {
                            Console.WriteLine("\n Connection Successful! \n");
                        }
                        Console.WriteLine("\n-Press any key to continue to the main menu.");
                        Console.ReadKey();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error connecting to Database.");
                    }
                    break;



                // Close the connection
                case "B":
                    try
                    {
                        Console.WriteLine("Closing the connection...");
                        sql.Close();
                        Console.WriteLine("The connection is currently set to {0}. \n-Press any key to continue to the main menu", sql.State);
                        Console.ReadKey();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error in closing the connection to the database.");
                    }
                    break;



                // Check the Connection
                case "C":
                    try
                    {
                        Console.WriteLine("The current version: " + sql.ServerVersion);
                        Console.WriteLine("The current connection is {0}. \n-Press any key to continue to the main menu.", sql.State);
                        Console.ReadKey();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in checking the connection to the database.\nPress any key to continue back to the main menu.");
                        Console.ReadKey();
                    }
                    break;

                //Retrieve teacher information from classroom database
                case "R":
                    try
                    {
                        //Retrieve teacher ID from user
                        Console.WriteLine("Please enter the Teacher ID on the following line.");
                        string inputID = Console.ReadLine();
                        int teacherID = Convert.ToInt32(inputID);
                        Console.WriteLine("\nYou entered the Teacher ID: " + teacherID + "\nPress any key to continue..");
                        Console.ReadKey();
                        //Create SQL Query
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "SELECT Teacher_FName, Teacher_LName FROM Teachers WHERE Teacher_ID = " + teacherID;
                        command.Connection = sql;
                        //In case its not open
                        sql.Open();

                        //Retrieve information from DB
                        SqlDataReader reader;
                        reader = command.ExecuteReader();

                        // Call Read before accessing data.
                        reader.Read();

                        //Create a teacher object passing the two strings obtained from the database query
                        Teacher teacher = new Teacher(reader[0].ToString(), reader[1].ToString());

                        //Use the overrided method of the teacher class to print the teacher's information
                        teacher.ToString();

                        // Call Close when done reading.
                        reader.Close();
                        sql.Close();
                        Console.ReadKey();

                        //Create 


                    }
                    catch(Exception e)
                    {
                        //Reset the connection
                        try
                        {
                            sql.Close();
                        }
                        catch { }
                        
                        Console.WriteLine("There was an error in requesting the information..\n Either the information does not exist in the database, or there is a connection problem,\n Check information and connection and try agan.\n\nPress any key to continue back to the main menu");
                        Console.ReadKey();
                    }
                    break;

                // Exit the program
                case "X":
                    try
                    {
                        Console.WriteLine("Closing the connection...");
                        sql.Close();
                        Console.WriteLine("The connection is currently set to {0}. \n-Press any key to continue to exit.", sql.State);
                        Console.ReadKey();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("Error in closing the connection to the database.\n Exception:     " + e);
                    }
                    Environment.Exit(0);
                    break;



                // Default for the Menu is an error message
                default:
                    Console.WriteLine("Invalid Input! \n(Press any key to return to main menu)");
                    Console.ReadKey(); Console.Clear();
                    break;
            }
        }

        //Displays the main menu

        public static void displayMenu()
        {
            if (sql.State.ToString() == "Open")
            {
                Console.WriteLine("Sql Connection Open.");
                Console.WriteLine(sql.ConnectionString);
                Console.WriteLine(sql.ServerVersion);
                Console.WriteLine("------------------------\n");
            }

            Console.WriteLine("Main Menu");
            Console.WriteLine("A. Create Connection \n"
                             + "B. Close Connection  \n"
                             + "C. Check Connection  \n"
                             + "R. Retrieve Teacher Information From Database\n"
                             + "X. Exit Program      \n");
        }

        //A function to prompt to continue (Redundant)
        public static void promptToContinue()
        {
            Console.WriteLine("Press any key to continue (type \"x\" or \"exit\" to exit).");
            string key = Console.ReadLine();
            key = key.ToUpper();
            if (key == "EXIT" || key == "X") { keepGoing = false; }
        }

        //A function to switch the loop controller condition
        public static void switchKeepGoing()
        {
            keepGoing = !keepGoing;
        }
    }

    //A teacher class
    public class Teacher
    {
        //Contains two properties
        string firstName, lastName;

        //Default Constructor
        public Teacher()
        {
            firstName = "";
            lastName = "";
        }
        //Second Constructor that accepts two strings (teacher's first and last name)
        public Teacher(string a, string b)
        {
            firstName = a;
            lastName = b;
        }
        //Appropriate Getters for both properties (teacher's first and last name)
        public string getFirstName()
        {
            return firstName;
        }
        public string getLastName()
        {
            return lastName;
        }
        //Appropriate Setters for both properties (teacher's first and last name)
        public void setFirstName(string a)
        {
            firstName = a;
        }
        public void setLastName(string b)
        {
            lastName = b;
        }
        //Overrided ToString() method to display two properties (teacher's first and last name)
        public void ToString()
        {
            Console.WriteLine("\n---Teacher---\nFirst Name: " + firstName + ", Last Name: " + lastName);
        }

    }
}
