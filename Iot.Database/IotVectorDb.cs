using Iot.Database.Table;
using Iot.Database.Vector;
using Microsoft.ML;

namespace Iot.Database
{
    public class IotVectorDb<T> where T : IotValue
    {
        TableCollection<T> _collection;
        private readonly MLContext _mlContext;
        private readonly ITransformer _textFeaturizer;

        public IotVectorDb(string path = "", string name = "", string password = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iotdb");
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException(path);
            if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
            _collection = new(path, name, password);

            // Initialize ML.NET context
            _mlContext = new MLContext();

            // Define the ML.NET pipeline for text featurization
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(VectorData.Text));
            _textFeaturizer = pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new List<VectorData>()));
        }

        public void CreateAsync(T item)
        {
            // Generate embedding for item
            string serializedItem = Newtonsoft.Json.JsonConvert.SerializeObject(item);

            var jsonData = new List<VectorData> { new VectorData { Text = serializedItem } };

            var dataView = _mlContext.Data.LoadFromEnumerable(jsonData);
            var transformedData = _textFeaturizer.Transform(dataView);

            // Extract the embedding
            var features = _mlContext.Data.CreateEnumerable<TransformedVectorData>(transformedData, reuseRowObject: false).FirstOrDefault();
            if (features?.Features != null)
            {
                item.Embedding = features.Features.ToList();
            }
            
            // Save the Monster object to IotCollection
            var dbItem = _collection.FindOne(x=>x.Guid.Equals(item.Guid, StringComparison.OrdinalIgnoreCase));
            if (dbItem != null)
            {//update
                dbItem.CopyFrom(item);
                _collection.Update(dbItem);
            }
            else
            {//new
                _collection.Insert(item);
            }
        }

        public async Task<List<T>> Search(float[] embedding)
        {
            // Perform vector similarity search in memory
            var allItems = _collection.FindAll().ToList();

            // Calculate cosine similarity or other similarity metric
            var relevantItems = allItems
                .Select(item => new
                {
                    Item = item,
                    Similarity = CalculateCosineSimilarity(item.Embedding?.ToArray(), embedding)
                })
                .Where(x => x.Similarity > 0.9) // Set threshold as needed
                .OrderByDescending(x => x.Similarity)
                .Take(10)
                .Select(x => x.Item)
                .ToList();

            // Clean up unnecessary data
            foreach (var item in relevantItems)
            {
                item.Embedding = null;
            }

            return await Task.FromResult(relevantItems);
        }

        private double CalculateCosineSimilarity(float[]? vectorA, float[]? vectorB)
        {
            if (vectorA == null || vectorB == null || vectorA.Length != vectorB.Length)
                return 0;

            double dotProduct = 0, magnitudeA = 0, magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }

        

    }
}
