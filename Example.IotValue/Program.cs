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

            var point = new IotPoint();
            point.Name = "";
        }
    }
}
