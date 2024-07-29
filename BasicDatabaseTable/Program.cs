using ExampleShared;
using Iot.Database;

namespace BasicDatabaseTable;

internal class Program
{
    static void Main(string[] args)
    {
       

        // Specify database name and path
        var dbName = "MyIotDatabase";
        var dbPath = @"c:\temp";

        // Create an instance of IoTData
        var iotData = new IotDatabase(dbName, dbPath, "encryption password");

        // Create a table with the class name as the table name
        var friendTbl = iotData.Tables<Friend>();

        //Fine one record in the database table friend name Bob
        var friend = friendTbl.FindOne(x => x.Name.Equals("bob", StringComparison.OrdinalIgnoreCase));
        if (friend == null)
        { //bod doesn't exist

            //create a new friend
            friend = new Friend() { Name = "Bob" };

            //insert friend to database table 
            var id = friendTbl.Insert(friend);

            //successfull insert return the Id of the new record.
            //The insert also update the friend variable's Id property.
            if (id.IsNull)
            {
                Console.WriteLine("Failed to insert.");
                return;
            }
        }

        //display record
        Console.WriteLine($"Success: Id [{friend.Id}] Name [{friend.Name}]");

        //************************** Table Name *************************


        // Create another table with the class name as the table name but called it Best Friend
        var bestFriendTbl = iotData.Tables<Friend>("Best Friend");

        // Fine one record in the database table friend name Bob
        var bff = bestFriendTbl.FindOne(x => x.Name.Equals("bob", StringComparison.OrdinalIgnoreCase));
        if (bff == null)
        { //bod doesn't exist

            //create a new friend
            bff = new Friend() { Name = "Bob" };

            //insert friend to database table 
            var id = bestFriendTbl.Insert(bff);

            //successfull insert return the Id of the new record.
            //The insert also update the friend variable's Id property.
            if (id.IsNull)
            {
                Console.WriteLine("Failed to insert.");
                return;
            }
        }

        //display record
        Console.WriteLine($"Best Friend table. Success: Id [{bff.Id}] Name [{bff.Name}]");
    }
}
