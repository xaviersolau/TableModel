# TableModel
Library providing a way to define table model and to apply paging sorting and filtering operation
on server side.

Don't hesitate to post issues, pull requests on the project or to fork and improve the project.

## Project dashboard

[![Build - CI](https://github.com/xaviersolau/TableModel/actions/workflows/build-ci.yml/badge.svg)](https://github.com/xaviersolau/TableModel/actions/workflows/build-ci.yml)
[![Coverage Status](https://coveralls.io/repos/github/xaviersolau/TableModel/badge.svg?branch=main)](https://coveralls.io/github/xaviersolau/TableModel?branch=main)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

| Package                             | Nuget.org | Pre-release |
|-------------------------------------|-----------|-------------|
|**SoloX.TableModel**                 |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.TableModel.svg)](https://www.nuget.org/packages/SoloX.TableModel)|[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.TableModel.svg)](https://www.nuget.org/packages/SoloX.TableModel)|
|**SoloX.TableModel.Server**          |[![NuGet Beta](https://img.shields.io/nuget/v/SoloX.TableModel.Server.svg)](https://www.nuget.org/packages/SoloX.TableModel.Server)|[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.TableModel.svg)](https://www.nuget.org/packages/SoloX.TableModel)|

## License and credits

BlazorLayout project is written by Xavier Solau. It's licensed under the MIT license.

 * * *

## Installation

You can checkout this Github repository or you can use the NuGet packages:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.TableModel -version 1.0.0-alpha.7
Install-Package SoloX.TableModel.Server -version 1.0.0-alpha.7
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.TableModel --version 1.0.0-alpha.7
dotnet add package SoloX.TableModel.Server --version 1.0.0-alpha.7
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.TableModel" Version="1.0.0-alpha.7" />
<PackageReference Include="SoloX.TableModel.Server" Version="1.0.0-alpha.7" />
```

## Use case

Let's say that you write an application that needs to get some data from a server API. Usually
you start thinking that you just need the load the data and to send it directly to the client.
Then you realize that you gonna need to make a query with a filter on a given property but
your boss is pushing you to deliver as soon as possible so you just add a Query strings and use it
in your Entity Framework query through some custom methods in your repository.
Finally, you end up with multiple query string and many custom methods to implement all specific
requests.

Well, this is not really the best solution in terms of scalability and or maintainability. Don't
you agree?

This project is here to help you in a very easy way to add an end-point that is going to serve
your data and to be able to filter and/or sort your data. In addition, it's also going to help you
to make partial data request if you don't need to load all data at once!

Another nice point is that from the client side this project will allow you to make data filter
as easy as writing a lambda expression!

## How to use it

Note that you can find code examples in this repository at this location: `src/examples`.

### Create a shared Dto assembly

First of all, the Dto objects must be shared between the server and the client.

For instance if you want to use a `WeatherForecastDto`:
* The server serving the data will need to have a reference on it to define the data mapping between
the internal entities and the transmitted Dto.
* The client requesting the data will need to have a reference on it to register the TableData and to
use it. Thanks to Blazor this is also possible on web client side.

### Set up the server

#### Add the Nuget

On the server API, you can add the `SoloX.TableModel.Server` Nuget. It provides all TableModel support
and the controller base implementations.

#### Register services

In order to set up the TableModel services to serve table data, you just need to call the
extension method `AddTableModelServer` from `SoloX.TableModel.Server` name space.

Here is a quick example to set up some data (in our case `WeatherForecastDto`) to serve from a custom
table data `WeatherForecastTableData`.

```csharp
services.AddTableModelServer(
    builder =>
    {
        // Here we are going to serve some data from a custom ITableData implementation.
        builder.UseQueryableTableData<WeatherForecastDto, WeatherForecastTableData>();
    });
```

Let's say that our `WeatherForecastTableData` will load our data from an Entity Framework
DdContext `WeatherForecastDbContext`. All you need to do to write your `WeatherForecastTableData`
is to define the mapping between your entity and your DTO.

For instance if we have an entity `WeatherForecastEntity` and a DTO `WeatherForecastDto` the
TableData will look like this:

```csharp
public class WeatherForecastTableData : AQueryableTableData<WeatherForecastDto>
{
    private readonly WeatherForecastDbContext dbContext;

    public WeatherForecastTableData(WeatherForecastDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    protected override IQueryable<WeatherForecastDto> QueryData()
    {
        return this.dbContext.WeatherForecasts
            .Select(enity => new WeatherForecastDto()
            {
                /* Initialize all properties from the entity... */
            });
    }
}
```

#### Create the TableData Controller

Now that our TableData is initialized, we have to create the Controller that will actually
serve the data through the server API end-point.

Once again it is really easy to make it because we just have to create a controller class
based on the provided `TableDataControllerBase` implementation. You also have to declare the
appropriate attributes to make your controller available through ASP.Net Core API stack.

```csharp
/// <summary>
/// The TableDataController based on TableDataControllerBase that is providing the end-points to
/// query table data.
/// </summary>
/// <remarks>
/// You can also use the Authorization attributes here.
/// </remarks>
[Route("api/[controller]")]
[ApiController]
public class TableDataController : TableDataControllerBase
{
    /// <summary>
    /// Setup the TableDataController with the tableDataEndPointService provided in the
    /// SoloX.TableModel.Server package.
    /// </summary>
    /// <param name="tableStructureEndPointService">The end-point service that is actually doing the job.</param>
    public TableDataController(ITableDataEndPointService tableDataEndPointService)
        : base(tableDataEndPointService)
    {
    }
}
```

Basically this controller will provide 3 end-points:

| Url path | HTTP Method | Summary | Result |
|---|----|---|---|
| /api/TableData | GET | Get the list of table data available. | The table data list. |
| ​/api​/TableData​/\{*table data id*\}/data | POST | Post a data request. | The requested data items. |
| ​/api​/TableData​/\{*table data id*\}/count | POST | Post a data count request. | The data count. |

#### Create the TableStructure Controller

Coming soon...

### Set up the client

#### Add the Nuget

On the client, you can add the `SoloX.TableModel` Nuget. It provides all TableModel support.

#### Register services

In order to set up the TableModel services to use the table data, you just need to call the
extension method `AddTableModel` from `SoloX.TableModel` name space.

Here is a quick example to set up some data (in our case `WeatherForecastDto`) to be requested from
the server.

In the `Program.cs` file form the Wasm Blazor client, you can register the TableModel services like this:

```csharp
builder.services.AddTableModel(
    tableBuilder =>
    {
        tableBuilder.UseRemoteTableData<WeatherForecastDto>(
            config =>
            {
                config.HttpClient = new HttpClient
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/TableData/")
                };
            });
    });
```

### Using the TableData

Whether or not we are in the server or the client project, the TableData can be used the same way.

#### Get TableData from ITableDataRepository

Once the table is registered in the services configuration, you just need to inject the `ITableDataRepository`.
From this repository we can get a `TableData` instance associated to a given data type.

> 
> Note that creating a TableData instance does not actually load any data. The data will be loaded at query time.
>

Let's have a look at how we get a `TableData` instance with an example:

```csharp
// Inject the TableDataRepository to get the registered TableData.
// It also can be injected as a constructor parameter.
[Inject]
public ITableDataRepository TableDataRepository { get; set; }

// Let's Load some data.
public async Task<IEnumerable<WeatherForecastDto>> LoadDataAsync()
{
    // Get the TableData associated to the type WeatherForecastDto from the TableDataRepository type.
    var weatherForecastTableDate = await TableDataRepository.GetTableDataAsync<WeatherForecastDto>();

    /// Then load data...
}
```

#### Basic queries

Now that we have got our TableData instance, we can make data queries to get the whole data or just a data page:

```csharp
// First get data count from the TableData.
var dataCount = await weatherForecastTableDate.GetDataCountAsync();

// Get all data from the TableData.
var allData = await weatherForecastTableDate.GetDataAsync();

// But we may only need a data page from a given data index and page size.
var someDataPage = await weatherForecastTableDate.GetDataPageAsync(index, size);
```

#### Sorting data

If you need to get sorted data on a given property, thanks to the Lambda expression, it's really easy:

```csharp
// Inject the ITableFactory to get a ITableSorting instance.
// It also can be injected as a constructor parameter.
[Inject]
public ITableFactory TableFactory { get; set; }

// Let's Load some sorted data.
public async Task<IEnumerable<WeatherForecastDto>> LoadSortedDataAsync(
    ITableData<WeatherForecastDto> weatherForecastTableDate)
{
    // Create the TableSorting.
    var tableSorting = TableFactory.CreateTableSorting<WeatherForecastDto>();

    // Register a sorting operation on TemperatureC property.
    tableSorting.Register(wf => wf.TemperatureC, SortingOrder.Ascending);

    // Run the query using the tableSorting.
    var sortedData = weatherForecastTableDate.GetDataAsync(tableSorting);
}
```

#### Filter data

Here again, it's easy to use lambda to setup the data filters:

```csharp
// Inject the ITableFactory to get a ITableFilter instance.
// It also can be injected as a constructor parameter.
[Inject]
public ITableFactory TableFactory { get; set; }

// Let's Load some filtered data.
public async Task<IEnumerable<WeatherForecastDto>> LoadFilteredDataAsync(
    ITableData<WeatherForecastDto> weatherForecastTableDate)
{
    // Create the TableFilter.
    var tableFilter = TableFactory.CreateTableFilter<WeatherForecastDto>();

    // Register a filter operation on Summary property.
    tableFilter.Register(wf => wf.Summary, s => s.Contains("Hot"));

    // Run the query using the tableFilter.
    var filteredData = weatherForecastTableDate.GetDataAsync(tableFilter);
}
```
