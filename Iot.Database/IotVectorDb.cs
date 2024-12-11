using Iot.Database.Table;
using System.Collections.Concurrent;

namespace Iot.Database
{
    public class IotVectorDb<T> where T : IotValue
    {
        private readonly object _queueLock = new object();
        private readonly TableCollection<T> _collection;
        private readonly Func<object, Task<float[]?>> _embeddingFunction;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public IotVectorDb(Func<object, Task<float[]?>> embeddingFunction, string path = "", string name = "", string password = "")
        {
            _embeddingFunction = embeddingFunction ?? throw new ArgumentNullException(nameof(embeddingFunction));

            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iotdb");
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException(path);
            if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
            _collection = new(path, name, password);

            // Start the background task to process the queue
            Task.Run(ProcessQueueAsync);
        }

        public void QueueCreate(T item)
        {
            lock (_queueLock)
            {
                // Create a temporary queue to hold items that don't match the Guid
                var tempQueue = new ConcurrentQueue<T>();

                while (_queue.TryDequeue(out var existingItem))
                {
                    if (!existingItem.Guid.Equals(item.Guid, StringComparison.OrdinalIgnoreCase))
                    {
                        tempQueue.Enqueue(existingItem);
                    }
                }

                // Enqueue the new item
                tempQueue.Enqueue(item);

                // Reassign the original queue to the filtered queue
                while (tempQueue.TryDequeue(out var tempItem))
                {
                    _queue.Enqueue(tempItem);
                }
            }
        }

        public int GetQueueCount()
        {
            return _queue.Count;
        }

        private async Task ProcessQueueAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                T item = default;
                bool hasItem = false;

                lock (_queueLock)
                {
                    if (_queue.TryDequeue(out item))
                    {
                        hasItem = true;
                    }
                }

                if (hasItem)
                {
                    await CreateAsync(item);
                }
                else
                {
                    await Task.Delay(100); // Wait for a short period before checking the queue again
                }
            }
        }

        private async Task CreateAsync(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                bool newItem = true;
                Guid id = Guid.Empty;
                var dbItem = _collection.FindOne(x => x.Guid.Equals(item.Guid, StringComparison.OrdinalIgnoreCase));
                if (dbItem != null && dbItem.Timestamp == item.Timestamp) return; // item has not changed
                if (dbItem == null)
                {
                    newItem = true;
                    dbItem = (T)(new IotValue());
                }
                else
                {
                    newItem = false;
                    id = dbItem.GetIotDbId() ?? Guid.Empty;
                }

                dbItem.CopyFrom(item);

                // Generate embedding for item
                var embedding = await _embeddingFunction(item);

                if (embedding == null) return;

                dbItem.Embedding = embedding.ToList();

                if (newItem)
                { // new
                    _collection.Insert(dbItem);
                }
                else
                { // update
                    dbItem.SetIotDbId(id);
                    _collection.Update(dbItem);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<T>> Search(string searchText)
        {
            var embedding = await _embeddingFunction(searchText);

            if (embedding != null)
            {
                return await Search(embedding);
            }

            return new();
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
