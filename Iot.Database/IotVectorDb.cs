using FaissNet;
using Iot.Database.IotValueUnits;
using Iot.Database.Table;
using System.Collections.Concurrent;

namespace Iot.Database;

public class IotVectorDb<T> where T : ValueBase
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

    #region Get

    public ITableCollection<T> Collection => _collection;
    #endregion

    public void InsertUpdateQueue(T item)
    {
        lock (_queueLock)
        {
            // Create a temporary queue to hold items that don't match the Guid
            var itemInQueue = _queue.Any(x => x.Guid.Equals(item.Guid, StringComparison.CurrentCultureIgnoreCase));
            if (itemInQueue)
            {
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
            else
            {
                _queue.Enqueue(item);
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
                await InsertUpdate(item);
            }
            else
            {
                await Task.Delay(100); // Wait for a short period before checking the queue again
            }
        }
    }

    private async Task InsertUpdate(T item)
    {
        await _semaphore.WaitAsync();
        try
        {
            bool newItem = true;
            Guid id = Guid.Empty;
            var dbItem = _collection.FindOne(x => x.Guid.Equals(item.Guid, StringComparison.OrdinalIgnoreCase));
            if (dbItem != null && dbItem.JsonTimestamp == item.JsonTimestamp) return; // item has not changed
            if (dbItem == null)
            {
                newItem = true;
                dbItem = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                newItem = false;
                id = dbItem.GetIotDbId() ?? Guid.Empty;
            }

            dbItem.CopyFrom(item);

            // Generate embedding for item
            var embedding = await _embeddingFunction(dbItem);

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
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<T>> SearchFaissTopKAsync(string searchText, int k = 10)
    {
        var embedding = await _embeddingFunction(searchText);

        if (embedding != null && embedding.Length > 0)
        {
            return await SearchFaissTopKAsync(embedding, k);
        }

        return new();
    }

    public async Task<List<T>> SearchFaissTopKAsync(float[] queryVector, int k = 10)
    {
        int dimension = queryVector.Length; // Dimensionality of vectors
        var allItems = _collection.FindAll().ToList();
        int dataSize = allItems.Count; // Number of vectors

        // Create an index with L2 metric
        using var index = FaissNet.Index.CreateDefault(dimension, MetricType.METRIC_L2);

        // Prepare data for the index
        float[][] data = new float[dataSize][];
        for (int i = 0; i < dataSize; i++)
        {
            data[i] = allItems[i].Embedding?.ToArray() ?? new float[dimension];
        }

        // Add data to the index
        index.Add(data);

        // Search for the top K nearest neighbors
        var (distances, ids) = index.Search(new[] { queryVector }, k);

        // Map the results back to the original items
        
        var result = ids[0].Select(id => allItems[(int)id]).ToList();

        return await Task.FromResult(result);
    }

    public async Task<List<T>> SearchFaissRadiusAsync(string searchText, float radius = 5.0f)
    {
        var embedding = await _embeddingFunction(searchText);

        if (embedding != null && embedding.Length > 0)
        {
            return await SearchFaissRadiusAsync(embedding, radius);
        }

        return new();
    }
    public async Task<List<T>> SearchFaissRadiusAsync(float[] queryVector, float radius)
    {
        int dimension = queryVector.Length; // Dimensionality of vectors
        var allItems = _collection.FindAll().ToList();
        int dataSize = allItems.Count; // Number of vectors

        // Create an index with L2 metric
        using var index = FaissNet.Index.CreateDefault(dimension, MetricType.METRIC_L2);

        // Prepare data for the index
        float[][] data = new float[dataSize][];
        long[] ids = new long[dataSize];
        for (int i = 0; i < dataSize; i++)
        {
            data[i] = allItems[i].Embedding?.ToArray() ?? new float[dimension];
            ids[i] = i; // Assign unique IDs to each vector
        }

        // Add data to the index with IDs
        index.AddWithIds(data, ids);

        // Perform a large search (k = dataSize)
        var (distances, searchIds) = index.Search(new[] { queryVector }, dataSize);

        // Filter results by radius
        var filteredResults = searchIds[0]
            .Zip(distances[0], (id, distance) => new { id, distance })
            .Where(x => x.distance <= radius)
            .Select(x => allItems[(int)x.id])
            .ToList();

        return await Task.FromResult(filteredResults);
    }

    /// <summary>
    /// Similarity value between -1 and 1. 1 indicates maximum similarity, 0 indicates no similarity, and  -1 indicates maximum dissimilarity
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="similarity"></param>
    /// <param name="maxVectorSearchResults"></param>
    /// <returns></returns>
    public async Task<List<T>> SearchCosSimilarityAsync(string searchText, double similarity = 0.55, int maxVectorSearchResults = 150)
    {
        var embedding = await _embeddingFunction(searchText);

        if (embedding != null && embedding.Length > 0)
        {
            return await SearchCosSimilarityAsync(embedding, similarity, maxVectorSearchResults);
        }

        return new();
    }

    /// <summary>
    /// Similarity value between -1 and 1. 1 indicates maximum similarity, 0 indicates no similarity, and  -1 indicates maximum dissimilarity
    /// </summary>
    /// <param name="embedding"></param>
    /// <param name="similarity"></param>
    /// <param name="maxVectorSearchResults"></param>
    /// <returns></returns>
    public async Task<List<T>> SearchCosSimilarityAsync(float[] embedding, double similarity = 0.55, int maxVectorSearchResults = 150)
    {
        // Perform a search based on the embedding
        var allItems = _collection.FindAll().ToList();
        var results = allItems
            .Select(item => new
            {
                Item = item,
                Similarity = CosineSimilarity(item.Embedding.ToArray(), embedding)
            })
            .Where(x=>x.Similarity >= similarity)
            .OrderByDescending(x => x.Similarity)
            .Take(maxVectorSearchResults)
            .Select(x => x.Item)
            .ToList();

        // Clean up unnecessary data to send to completions AI models
        foreach (var item in results)
        {
            item.Embedding = null;
        }

        return await Task.FromResult(results);
    }

    /// <summary>
    /// Similarity value between -1 and 1. 1 indicates maximum similarity, 0 indicates no similarity, and  -1 indicates maximum dissimilarity
    /// </summary>
    /// <param name="vectorA"></param>
    /// <param name="vectorB"></param>
    /// <returns></returns>
    private double CosineSimilarity(float[] vectorA, float[] vectorB)
    {
        double dotProduct = 0.0;
        double magnitudeA = 0.0;
        double magnitudeB = 0.0;

        for (int i = 0; i < vectorA.Length; i++)
        {
            dotProduct += vectorA[i] * vectorB[i];
            magnitudeA += Math.Pow(vectorA[i], 2);
            magnitudeB += Math.Pow(vectorB[i], 2);
        }

        magnitudeA = Math.Sqrt(magnitudeA);
        magnitudeB = Math.Sqrt(magnitudeB);

        if (magnitudeA == 0 || magnitudeB == 0)
        {
            return 0;
        }

        return dotProduct / (magnitudeA * magnitudeB);
    }

    
}
