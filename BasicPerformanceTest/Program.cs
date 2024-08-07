
using Example.Shared.Models;
using Iot.Database;
using System.Diagnostics;

namespace BasicPerformanceTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Specify database name and path
            var dbName = "MyIotDatabase";
            var dbPath = @"c:\temp";

            //delete the old database file
            var dbFile = Path.Combine(dbPath, dbName);
            if (File.Exists(dbFile)) File.Delete(dbFile);

            // Create an instance of IoTData
            var iotData = new IotDatabase(dbName, dbPath, "encryption password");

            // Create a table with the class name as the table name
            var friendTbl = iotData.Tables<Friend>();

            // Fine one record in the database table friend name Bob
            var friend = friendTbl.FindOne(x => x.Name.Equals("bob", StringComparison.OrdinalIgnoreCase));
            if (friend == null)
            { //bod doesn't exist

                //create a new friend
                friend = new Friend() { Name = "Bob" };

                //insert friend to database table 
                var id = friendTbl.Insert(friend);

                //successful insert return the Id of the new record.
                //The insert also update the friend variable's Id property.
                if (id.IsNull)
                {
                    Console.WriteLine("Failed to insert.");
                    return;
                }
            }

            //display record
            Console.WriteLine($"Success: Id [{friend.Id}] Name [{friend.Name}]");

            // Stress test variables
            int numRecords = 10000; // Number of records to insert and read
            Stopwatch stopwatch = new Stopwatch();

            // Measure write time
            stopwatch.Start();
            for (int i = 0; i < numRecords; i++)
            {
                var newFriend = new Friend() { Name = "Friend" + i };
                friendTbl.Insert(newFriend);
            }
            stopwatch.Stop();
            Console.WriteLine($"Time taken to write {numRecords} records: {stopwatch.ElapsedMilliseconds} ms");

            // Measure read time
            var count = friendTbl.Count();
            stopwatch.Restart();
            var allFriends = friendTbl.FindAll().ToList();
            stopwatch.Stop();
            Console.WriteLine($"Time taken to read {count} records: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
