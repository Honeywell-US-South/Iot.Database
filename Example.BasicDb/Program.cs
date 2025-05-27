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
            if (db.Tables<Customer>().Find(x => x.Id == 1) == null)
            {
                customerTable.Insert(new Customer { Id = 1, Name = "John", Age = 30 });
            }
            if (db.Tables<Customer>().Find(x => x.Id == 2) == null)
            {
                customerTable.Insert(new Customer { Id = 2, Name = "Jane", Age = 25 });
            }
                

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
            //var resultsProgrammatic = db.Query
            //    .Find<Customer>("Customer", c => true, "Name as Person", "Age")
            //    .Include<Order>("Order", o => o.Amount > 100, "Amount as Total")
            //    .Include<Address>("Address", a => a.AddressLine1.Contains("Main"), "AddressLine1 as Address")
            //    .Execute();
            //var resultsProgrammatic = db.Query
            //    .Find<Customer>("Customer", c=>true, "Name as Person", "Age")
            //    .Include<Order>("Order", o => o.Amount > 100, "Amount as Total")
            //    .Include<Address>("Address", a => a.AddressLine1.Contains("Main"), "AddressLine1 as Address")
            //    .Execute("Join as my table Select Person, Total, Address");
            var resultsProgrammatic = db.Query
               .Find<Customer, Order, Address>("Customer", "Order", "Address", c => c.Age > 20, o=>o.Amount > 100, a=>a.AddressLine1.Contains("Main"), new[] { "Name as Person", "Age" }, new[] {"Amount as Total"}, new[] { "AddressLine1 as Address" })
               .Execute();
            Console.WriteLine("Programmatic Query Results:");
            foreach (var result in resultsProgrammatic)
            {
                Console.WriteLine($"{result.TableName} Result: {result.Data}");
            }

            // Using new NaturalQuery method
            //var query = "FIND Customer,Order ON Order.CustomerId = Customer.Id WHERE Age > 20 AND Amount > 100 SELECT Id,Name as CustomerName,Id,OrderDate ORDER BY Name ASC LIMIT 10";
            var query = "FIND Customer,Order, Address WHERE Age > 20 AND Amount > 100 AND AddressLine1 CONTAINS Main SELECT Id,Name as CustomerName,Id,OrderDate, AddressLine1 as Address ORDER BY Name ASC LIMIT 10";
            //var resultsNatural = db.Query.NaturalQuery("FIND Customer WHERE Age > 0 INCLUDE Order WHERE Amount > 100 SELECT Amount, CustomerId INCLUDE Address Where AddressLine1 Contains Main Select AddressLine1 as Address JOIN as New Table Name select Name, Amount, Address");
            var resultsNatural = db.Query.NaturalQueryTriple(query);
            Console.WriteLine("\nNatural Query Results:");
            foreach (var result in resultsNatural)
            {
                Console.WriteLine($"Result: {result.Data}");
            }

        }
    }

}
