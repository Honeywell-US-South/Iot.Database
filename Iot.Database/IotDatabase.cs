using Iot.Database.FileManager;
using Iot.Database.Table;
using System.Collections.Concurrent;

namespace Iot.Database;

public class IotDatabase
{
    // Define the event based on the delegate
    public event EventHandler<ExceptionEventArgs>? ExceptionOccurred;

    private string _dbPath;
    private string _tsPath;
    private string _tbPath;
    private string _flPath;
    

    private ConcurrentDictionary<string, dynamic> _tables = new();
    internal ConcurrentDictionary<string, TableInfo> _tableInfos = new();

    private ConcurrentDictionary<string, IFileCollection> _files = new();
    internal readonly string _password;


    public IotDatabase(string dbName, string dbPath, string? password)
    {
        _password = password ?? "";
       
        // Directory checks and creation
        InitializeDirectories(dbName, dbPath);
        if (!Directory.Exists(_dbPath)) throw new DirectoryNotFoundException($"Unable to create database directory. {_dbPath}");
        if (!Directory.Exists(_tsPath)) throw new DirectoryNotFoundException($"Unable to create timeseries directory. {_tsPath}");
        if (!Directory.Exists(_tbPath)) throw new DirectoryNotFoundException($"Unable to create tables directory. {_tbPath}");
        if (!Directory.Exists(_flPath)) throw new DirectoryNotFoundException($"Unable to create files directory. {_flPath}");
       

    }

    #region Tables
    public string TableDbPath { get { return _tbPath; } }
    public ITableCollection<T> Tables<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if (!_tables.ContainsKey(tableName))
        {
            _tables[tableName] = new TableCollection<T>(this);
            ((TableCollection<T>)_tables[tableName]).ExceptionOccurred += OnExceptionOccurred;
        }
        
        return (ITableCollection<T>)_tables[tableName];
    }

    public ITableCollection<T> Tables<T>(string tableName) where T : class
    {
        tableName = $"{tableName}_{typeof(T).Name}";
        if (!_tables.ContainsKey(tableName))
        {
            _tables[tableName] = new TableCollection<T>(this, tableName);
            ((TableCollection<T>)_tables[tableName]).ExceptionOccurred += OnExceptionOccurred;
        }

        return (ITableCollection<T>)_tables[tableName];
    }

    internal ITableCollection? GetTable(string tableName)
    {
        if (_tables.ContainsKey(tableName))
        {
            return (ITableCollection)_tables[tableName];
            

        }
        return null;
    }

    #endregion

    public List<string> Resources
    {
        get
        {
            List<string> resources = new List<string>();
            foreach (var table in _tables)
            {
                resources.Add($"table_{table.Key}");
            }
            foreach (var file in _files)
            {
                resources.Add($"file_{file.Key}");
            }
            return resources;
        }
    }


    #region Files

    public IFileCollection Files(string containerName)
    {
        string name = containerName;
        if (!_files.ContainsKey(name))
        {
            _files[name] = new FileCollection(_flPath, name, _password);
            _files[name].ExceptionOccurred += OnExceptionOccurred;
        }

        return _files[name];
    }
    #endregion

    #region Base Functions
    private void InitializeDirectories(string dbName, string dbPath)
    {
        Helper.MachineInfo.CreateDirectory(dbPath);
         var dbPathName = Path.Combine(dbPath, dbName);
        _dbPath = dbPathName;
        Helper.MachineInfo.CreateDirectory(_dbPath);
        _tsPath = Path.Combine(_dbPath, "TimeSeries");
        Helper.MachineInfo.CreateDirectory(_tsPath);
        _tbPath = Path.Combine(_dbPath, "Tables");
        Helper.MachineInfo.CreateDirectory(_tbPath);
        _flPath = Path.Combine(_dbPath, "Files");
        Helper.MachineInfo.CreateDirectory(_flPath);
    }

    

    protected virtual void OnExceptionOccurred(object? sender, ExceptionEventArgs e)
    {
        ExceptionOccurred?.Invoke(sender ?? this, e);
    }

    #endregion

}
