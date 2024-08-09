using Example.Shared.Models;
using Iot.Database;

namespace Example.IotValue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Specify database name and path
            var dbName = "IotValueExample";
            var dbPath = @"c:\temp";

            // Create an instance of IoTData
            var iotData = new IotDatabase(dbName, dbPath, "encryption password");

            // Create a table with the class name as the table name
            var pointTbl = iotData.Tables<IotPoint>();
            string input = string.Empty;
            var point = new IotPoint();
            point.Name = "Hello";
            point.TimeSeries = true;
            point.SetValue15Default(15);
            var id = pointTbl.Insert(point);
            while (!input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                
                point.SetValue15Default(15);
                pointTbl.Update(point);
                var tsValue = pointTbl.GetTimeSeries(point, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
                input = Console.ReadLine();
            }
        }
    }
}
