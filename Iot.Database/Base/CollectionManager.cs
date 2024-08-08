namespace Iot.Database.Base
{
    internal class CollectionManager<T> where T : class
    {
        public string CollectionName { get; set; }
        public ILiteCollection<T> Collection { get; set; }

        public CollectionManager(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
