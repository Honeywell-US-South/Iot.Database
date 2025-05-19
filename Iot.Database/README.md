
# Iot.Database Library
## Current Version: 0.0.20-beta

## Overview

**IoT.Database** is a lightweight, embedded database library designed for IoT applications, providing a simple and intuitive interface for storing, querying, and managing data in a local database. It supports both programmatic and natural language-like queries, allowing developers to interact with data efficiently. The library is ideal for scenarios requiring local data storage with flexible querying capabilities, such as IoT devices, edge computing, or small-scale applications.

## Features

- **Embedded Database**: Runs locally without requiring a separate database server.
- **Table Management**: Create and manage tables for structured data (e.g., `Customer`, `Order`, `Address`).
- **Programmatic Queries**: Use a fluent API to query data with predicates and column aliases.
- **Natural Language Queries**: Execute SQL-like queries with `FIND`, `WHERE`, `SELECT`, `INCLUDE`, and `INNERJOIN` clauses.
- **Join Operations**: Perform inner joins on query results, supporting custom table names and column selection.
- **Data Encryption**: Secure data with an encryption password.
- **Flexible Column Selection**: Support for column aliases (e.g., `Name as Person`) and selecting all columns when none are specified.
- 
## Versioning

- Version format of X.Y.Z (Major.Minor.Patch).
- X.Y.Z-beta (version under beta test).
- X.Y.Z-rc.1 (version under release candidate 1).
- NuGet package release after completion of rc release.

## Goals

- Easy to use
- GPT Query Ready
- Lightweight
- Encryption
- Quick and easy for IoT development and deployment

## Before You Continue

- This is a beta release. 

## Installation

To use the Iot.Database library in your project, follow these steps:

1. .NET environment compatible with C# .NET 7
2. Install Iot.Database NuGet Package (not available until version 1.0.0).

## Quick Start

### Initializing the Database
Iot.Database stores data in flat files. Make sure your application has write permission to the database path.

```csharp
using Iot.Database;

// Specify database name and path
var dbName = "MyIotDatabase";
var dbPath = @"c:\temp";

// Create an instance of IoTData
var iotData = new IotDatabase(dbName, dbPath, "encryption password");
```
This creates an empty database in your c:\temp directory

![image](https://github.com/d42y/Iot.Database/assets/29101692/51d59c34-2c5f-4728-aa95-72769172b832)

### Using IoTDB Table
#### 1. Create a public class for your data structure
```csharp
public class Friend
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
```
The above class is the model for your table.
All tables must have an ID property. The Id property must be of type: Guid, int, or double

#### 2. Access the table
```csharp
static void Main(string[] args)
{
    // Specify database name and path
    var dbName = "MyIotDatabase";
    var dbPath = @"c:\temp";

    // Create an instance of IoTData
    var iotData = new IotDatabase(dbName, dbPath);

    // Create a table with the class name as the table name
    var friendTbl = iotData.Tables<Friend>();
}
```
This creates an empty table in the Tables folder.

![image](https://github.com/d42y/Iot.Database/assets/29101692/b614ae68-d48d-4ab7-ab6d-936b70d22b06)

What if you also want to create a table called BestFriend?
Create another derived class from friend.

![image](https://github.com/d42y/Iot.Database/assets/29101692/3a3f650f-fd27-4328-98c1-0bd9c4cf7b0e)

#### 3. Find and create a new record
```csharp
//check if the database has a friend name Bob
var friend = friendTbl.FindOne(x=>x.Name.Equals("bob", StringComparison.OrdinalIgnoreCase));
if (friend == null )
{
    //create a new friend
    friend = new Friend() { Name = "Bob" };
    //insert friend to database
    var id = friendTbl.Insert(friend);
    if (id.IsNull)
    {
        Console.WriteLine("Failed to insert.");
        return;
    } 
}

//display record
Console.WriteLine($"Success: Id [{friend.Id}] Name [{friend.Name}]");
```

![image](https://github.com/d42y/Iot.Database/assets/29101692/0f78a0b8-4c37-4fc4-9aa8-67dc73e9fb12)


#### 4. Foreign Key
```csharp
public class Address
{
    public Guid Id { get; set; }
    [TableForeignKey(typeof(Friend), TableConstraint.Cascading, RelationshipOneTo.One, "Each friend only have one address." )]
    public Guid FriendId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
```
The address class references the Friend table. 
IoTDB supports FK constraints, which allow you to cascade deletion and other actions. You can also set a one-to-one or one-to-many relationship.

```csharp
// Address table
var addressTbl = iotData.Tables<Address>();

//check if the database has a friend named Bob
var address = addressTbl.FindOne(x => x.FriendId == friend.Id);

if (address == null)
{
    //create a new friend
    address = new Address() { 
        FriendId = friend.Id,
        Street = "123 Main St.",
        City = "Friend Town",
        State = "TX",
        ZipCode = "75001-0001"
    };
    try
    {
        //insert friend to database
        var id = addressTbl.Insert(address);
        if (id.IsNull)
        {
            Console.WriteLine("Failed to insert.");
            return;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error insert address: [{ex.Message}]");
        return;
    }
}

//display record
Console.WriteLine($"Success: Id [{address.Id}] FriendId [{friend.Id}] Street [{address.Street}]");
```

![image](https://github.com/d42y/Iot.Database/assets/29101692/c138ecb6-4afd-477f-bdd5-ac1e306e8eff)

#### 5. Foreign Key Constraint Error
IoTDB throws an error for all contraint errors.
```csharp
//This throws an exception because of the one-to-one relationship. Only one address is allowed for each FK reference.
//[TableForeignKey(typeof(Friend), TableConstraint.Cascading, RelationshipOneTo.One, "Each friend only has one address." )]
//public Guid FriendId { get; set; }
var address2 = new Address()
{
    FriendId = friend.Id,
    Street = "789 ABC Street",
    City = "Friend Town",
    State = "TX",
    ZipCode = "75001-0001"
};
try
{
    //insert friend to database
    var id = addressTbl.Insert(address);
    if (id.IsNull)
    {
        Console.WriteLine("Failed to insert.");
        return;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error insert address: [{ex.Message}]");
    return;
}
```
![image](https://github.com/d42y/Iot.Database/assets/29101692/a68cb6a1-0d9e-415b-9799-083a0047c0f4)


### Initialize Database Tables
Database initialization is highly recommended for IoTDB applications with FK. 
Initilize parent table first.
```csharp
iotData.Tables<Friend>();
iotData.Tables<Address>();
```

### Querying Data

#### Programmatic Queries

Use the `QueryEngine` to build queries programmatically with the `Find` and `Include` methods. Specify predicates, column aliases, and join operations:

```csharp
var resultsProgrammatic = db.Query
    .Find<Customer>("Customer", c => c.Age > 25, "Name as Person", "Age")
    .Include<Order>("Order", o => o.Amount > 100, "Amount as Total")
    .Include<Address>("Address", a => a.AddressLine1.Contains("Main"), "AddressLine1 as Address")
    .Execute("InnerJoin as My Table Select Person, Total, Address");
```

- **Output**:
  ```json
{"my table_Data":[{"Person":"John","Total":{"$numberDecimal":"200.00"}},{"Person":"John","Address":"123 Main St"}]}
  ```

#### Natural Language Queries

Execute SQL-like queries with the `NaturalQuery` method, supporting `FIND`, `WHERE`, `SELECT`, `INCLUDE`, and `INNERJOIN` clauses:

```csharp
var resultsNatural = db.Query.NaturalQuery("FIND Customer WHERE Age > 25 and name startswith 'j' INCLUDE Order WHERE Amount > 150 SELECT Amount, CustomerId INNERJOIN as New Table Name select Name, Amount");
```

- **Output**:
  ```json
  {"New Table Name_Data":[{"Name":"John","Amount":{"$numberDecimal":"200.00"}}]}
  ```

### Displaying Results

Iterate over query results to display the data:

```csharp
Console.WriteLine("Programmatic Query Results:");
foreach (var result in resultsProgrammatic)
{
    Console.WriteLine($"{result.TableName} Result: {result.Data}");
}

Console.WriteLine("\nNatural Query Results:");
foreach (var result in resultsNatural)
{
    Console.WriteLine($"Result: {result.Data}");
}
```

## Query Syntax

### Programmatic Query
- **Find**: Specify the table, predicate, and columns (with optional aliases).
  ```csharp
  .Find<Customer>("Customer", c => c.Age > 25, "Name as Person", "Age")
  ```
- **Include**: Add related tables with predicates and columns.
  ```csharp
  .Include<Order>("Order", o => o.Amount > 100, "Amount as Total")
  ```
- **Execute**: Perform the query, optionally with a join command.
  ```csharp
  .Execute("InnerJoin as My Table Select Person, Total")
  ```

### Natural Query
- **Syntax**: `FIND <table> [WHERE <condition>] [SELECT <columns>] [INCLUDE <related_table> WHERE <related_condition> [SELECT <related_columns>]] [INNERJOIN [as <tableName>] select <columns>] [ORDER BY <column> [ASC|DESC]] [LIMIT <n>]`
- **Example**: `FIND Customer WHERE Age > 25 and name startswith 'j' INCLUDE Order WHERE Amount > 150 SELECT Amount, CustomerId INNERJOIN as New Table Name select Name, Amount`
- **Supported Operators**: `=`, `!=`, `>`, `<`, `>=`, `<=`, `contains`, `startswith`, `endswith`, `not contains`, `not startswith`, `not endswith`, `is null`, `is not null`, `is empty`.

## Join Operations

- **Inner Join**: Combines records from the primary table (e.g., `Customer`) and related table (e.g., `Order`) based on a foreign key (e.g., `Customer.Id` and `Order.CustomerId`).
- **Programmatic**: Use `Execute("InnerJoin [as <tableName>] Select <columns>")` to join results.
- **Natural**: Use `INNERJOIN [as <tableName>] select <columns>` in the query string.
- **Multi-Word Table Names**: Supported (e.g., `New Table Name`), allowing flexible naming for joined tables.

## Error Handling

- **Exceptions**: The library throws `ArgumentException` for invalid query formats, `InvalidOperationException` for missing methods or relationships, and custom exceptions for database errors.
- **Event**: Subscribe to `ExceptionOccurred` to handle errors:
  ```csharp
  db.Query.ExceptionOccurred += (sender, args) => Console.WriteLine($"Error: {args.Exception.Message}");
  ```

## Limitations

- **Single Include for Joins**: Currently supports joining one related table in `INNERJOIN` operations.
- **Column Aliases**: The `INNERJOIN select` clause uses original column names or aliases defined in `Find`/`Include`, not new aliases.
- **Foreign Key Assumption**: Joins assume a foreign key relationship (e.g., `CustomerId` in `Order` linking to `Id` in `Customer`).

## Closing or Unloading IoTDB
Unloading or closing IoTDB is not necessary. The library handles closure and recovery automatically. However, incomplete or unwritten data will be lost if your program ends or crashes during a data write.


## Contributing

Creating a robust and user-friendly database equivalent library requires significant effort and contributions from the public. However, I do not plan to accept outside contributions during the initial beta testing phase.


## License
This library is licensed under the MIT License. This means you are free to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the software, provided you include the following conditions in your distribution:

1. The software must include A copy of the original MIT License and copyright notice.
2. The software is provided "as is" without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose, and non-infringement. In no event shall the authors or copyright holders be liable for any claim, damages, or other liability, whether in an action of contract, tort, or otherwise, arising from, out of, or in connection with the software or the use or other dealings in the software.


This permissive license encourages open and collaborative software development while providing protection for the original authors. For more details, please refer to the full MIT License text.

## Third-Party Licenses and Acknowledgments

This software includes and/or depends on the following third-party software component, which is subject to its own license:
- **LiteDb**:  A .NET NoSQL Document Store database in a single data file. License MIT: For specific license terms, please refer to the [LiteDB github](https://github.com/mbdavid/LiteDB/blob/master/LICENSE).
- **TeaFile**: TeaFile is used for efficient time series data storage and access. License MIT: For specific license terms, please refer to the [TeaFile github](https://github.com/discretelogics/TeaFiles.Net-Time-Series-Storage-in-Files/blob/master/LICENSE).

We thank the contributors and maintainers of LiteDB and TeaFile for their work.

