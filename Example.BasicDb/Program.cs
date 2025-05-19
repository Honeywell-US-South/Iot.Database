using Example.Shared.Models;
using Iot.Database;

namespace Example.BasicDb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Specify database name and path
            var dbName = "MyIotDatabase";
            var dbPath = @"c:\temp";

            // Create an instance of IoTData
            var db = new IotDatabase(dbName, dbPath, "encryption password");
            
            // Populate some test data
            var customerTable = db.Tables<Customer>();
            //customerTable.Insert(new Customer { Id = 1, Name = "John", Age = 30 });
            //customerTable.Insert(new Customer { Id = 2, Name = "Jane", Age = 25 });

            var orderTable = db.Tables<Order>();
            //orderTable.Insert(new Order { Id = 1, CustomerId = 1, Amount = 150.00m });
            //orderTable.Insert(new Order { Id = 2, CustomerId = 1, Amount = 200.00m });
            //orderTable.Insert(new Order { Id = 3, CustomerId = 2, Amount = 100.00m });

            var addressTable = db.Tables<Address>();
            //addressTable.Insert(new Address { Id = 1, CustomerId = 1, AddressLine1 = "123 Main St" });
            //addressTable.Insert(new Address { Id = 2, CustomerId = 2, AddressLine1 = "456 Elm St" });

            // Join Customer and Order tables   
            // Query with Include
            // Using original Find/Include methods
            var resultsProgrammatic = db.Query
                .Find<Customer>("Customer", c => c.Age > 25, "Name as Person", "Age")
                .Include<Order>("Order", o => o.Amount > 100, "Amount as Total")
                .Include<Address>("Address", a => a.AddressLine1.Contains("Main"), "AddressLine1 as Address")
                .Execute("Join as my table Select Person, Total, Address");

            Console.WriteLine("Programmatic Query Results:");
            foreach (var result in resultsProgrammatic)
            {
                Console.WriteLine($"{result.TableName} Result: {result.Data}");
            }

            // Using new NaturalQuery method
            var resultsNatural = db.Query.NaturalQuery("FIND Customer WHERE Age > 25 and name startswith 'j' INCLUDE Order WHERE Amount > 150 SELECT Amount, CustomerId JOIN as New Table Name select Name, Amount");

            Console.WriteLine("\nNatural Query Results:");
            foreach (var result in resultsNatural)
            {
                Console.WriteLine($"Result: {result.Data}");
            }

        }
    }

}
