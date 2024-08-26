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
            point.SetValue15Default(0);
            var id = pointTbl.Insert(point);
            for (int i = 1; i < 10; i++) 
            {
                for (int j = 1; j <= 10; j++)
                {
                    point.SetValue15Default(j * i);
                    pointTbl.Update(point);
                    Thread.Sleep(1000);
                }

            }
            Thread.Sleep(10000);
            var tsValue = pointTbl.GetTimeSeries(point, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            input = Console.ReadLine();
        }
    }
}
