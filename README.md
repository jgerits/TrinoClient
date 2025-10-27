# TrinoClient

A .NET client library for connecting to and querying [Trino](https://trino.io/) (formerly PrestoSQL) clusters.

[![Build Status](https://github.com/jgerits/TrinoClient/actions/workflows/main.yml/badge.svg)](https://github.com/jgerits/TrinoClient/actions)
[![NuGet](https://img.shields.io/nuget/v/JGerits.TrinoClient.svg)](https://www.nuget.org/packages/JGerits.TrinoClient/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- Execute SQL queries against Trino clusters
- Support for authentication (basic auth)
- Retrieve query results in multiple formats (CSV, JSON, or raw data)
- Query management (list, get details, terminate queries)
- Node management (list active and failed nodes)
- JMX MBean support
- Thread monitoring
- Async/await support with cancellation tokens
- .NET 8.0 support

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package JGerits.TrinoClient
```

Or via Package Manager Console:

```powershell
Install-Package JGerits.TrinoClient
```

## Quick Start

### Basic Query Execution

```csharp
using TrinoClient;
using TrinoClient.Model.Statement;

// Configure the client
var config = new TrinoClientSessionConfig
{
    Host = "localhost",
    Port = 8080,
    Catalog = "hive",
    Schema = "default",
    User = "your-username"
};

// Create client instance
var client = new TrinodbClient(config);

// Execute a query
var request = new ExecuteQueryV1Request("SELECT * FROM my_table LIMIT 10");
var response = await client.ExecuteQueryV1(request);

// Process results
Console.WriteLine(string.Join("\n", response.DataToCsv()));
```

### Authentication with Password

```csharp
var config = new TrinoClientSessionConfig
{
    Host = "secure-trino.example.com",
    Port = 8443,
    Catalog = "hive",
    Schema = "default",
    User = "admin",
    Password = "your-password"
};

var client = new TrinodbClient(config);
```

### Working with Query Results

```csharp
var request = new ExecuteQueryV1Request("SELECT name, age FROM users");
var response = await client.ExecuteQueryV1(request);

// Get data as CSV
var csvData = response.DataToCsv();
foreach (var row in csvData)
{
    Console.WriteLine(row);
}

// Get data as JSON
var jsonData = response.DataToJson();
foreach (var item in jsonData)
{
    Console.WriteLine(item);
}

// Access raw data
var rawData = response.RawData;
```

### Query Management

```csharp
// List all queries
var queries = await client.GetQueries();

// Get details about a specific query
var queryDetails = await client.GetQuery("20231027_123456_00001_abcde");

// Kill a running query
await client.KillQuery("20231027_123456_00001_abcde");
```

### Node Information

```csharp
// List all nodes in the cluster
var nodes = await client.ListNodes();

// List failed nodes
var failedNodes = await client.ListFailedNodes();
```

### Using Cancellation Tokens

```csharp
var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(30));

try
{
    var request = new ExecuteQueryV1Request("SELECT * FROM large_table");
    var response = await client.ExecuteQueryV1(request, cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Query was cancelled");
}
```

## Configuration Options

The `TrinoClientSessionConfig` class supports the following properties:

| Property | Type | Description | Default |
|----------|------|-------------|---------|
| Host | string | Trino coordinator hostname or IP | localhost |
| Port | int | Trino coordinator port | 8080 |
| User | string | Username for authentication | Current user |
| Password | string | Password for basic authentication | null |
| Catalog | string | Default catalog to use | null |
| Schema | string | Default schema to use | default |
| Source | string | Source identifier for queries | null |
| ClientTags | HashSet<string> | Client tags for query tracking | null |
| Properties | IDictionary<string,string> | Session properties | null |
| IgnoreSslErrors | bool | Whether to ignore SSL certificate errors | false |

## Advanced Usage

### Custom Session Properties

```csharp
var config = new TrinoClientSessionConfig
{
    Host = "localhost",
    Port = 8080,
    Properties = new Dictionary<string, string>
    {
        { "query_max_memory", "1GB" },
        { "distributed_join", "true" }
    }
};
```

### Client Tags

```csharp
var config = new TrinoClientSessionConfig
{
    Host = "localhost",
    Port = 8080,
    ClientTags = new HashSet<string> { "reporting", "data-science" }
};
```

## Error Handling

The client provides specific exception types for different error scenarios:

- `TrinoException`: Base exception for Trino-related errors
- `TrinoQueryException`: Query execution errors
- `TrinoWebException`: HTTP/network errors

```csharp
try
{
    var response = await client.ExecuteQueryV1(request);
}
catch (TrinoQueryException ex)
{
    Console.WriteLine($"Query failed: {ex.Message}");
}
catch (TrinoWebException ex)
{
    Console.WriteLine($"Network error: {ex.Message}");
}
```

## Requirements

- .NET 8.0 or later
- Access to a Trino cluster (version 0.198 or later)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements

- Original fork from [PrestoClient](https://github.com/bamcis-io/PrestoClient) by BAMCIS
- Built for the [Trino](https://trino.io/) distributed SQL query engine

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/jgerits/TrinoClient).

## Revision History

### 1.0.0
- Migrated to .NET 8.0
- Updated for Trino compatibility
- Improved documentation
- Fixed compiler warnings
- Enhanced error handling

### 0.1
- Fork from [PrestoClient](https://github.com/bamcis-io/PrestoClient)