# TrinoClient

## Usage

### Basic Example 1

This demonstrates creating a new client config, initializing an ITrinoClient, and executing a simple query. The
returned data can be formatted in CSV or JSON. Additionally, all of the raw data is returned from the server
in case the deserialization process fails in .NET, the user can still access and manipulate the returned data.

```csharp
TrinoClientSessionConfig config = new TrinoClientSessionConfig("hive", "cars")
{
   Host = "localhost",
   Port = 8080
};

var client = new TrinodbClient(config);
var request = new ExecuteQueryV1Request("select * from tracklets limit 5;");
var queryResponse = await client.ExecuteQueryV1(request);

Console.WriteLine(String.Join("\n", queryResponse.DataToCsv()));
Console.WriteLine("-------------------------------------------------------------------");
Console.WriteLine(String.Join("\n", queryResponse.DataToJson()));
```

## Revision History

### 0.1
Fork from [PrestoClient](https://github.com/bamcis-io/PrestoClient)