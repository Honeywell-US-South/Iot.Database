using Iot.Database.Table;

namespace Iot.Database
{
    public class IotSimpleDb<T> where T : class
    {
        TableCollection<T> collection;

        public IotSimpleDb(string path = "", string name = "", string password = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iotdb");
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException(path);
            if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
            collection = new(path, name, password);
        }

        public ITableCollection<T> Collection { get { return collection; } }

    }
}
