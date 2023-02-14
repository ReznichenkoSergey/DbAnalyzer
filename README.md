## DbAnalyzer

This is a web service that allows you to analyze database objects based on the MSSQL Server DBMS and identify performance problems in the scanned database.
The service contains a number of methods, which can be divided into groups according to their purpose:
- **environment configuration methods**
The application's internal settings are used.
- **methods for obtaining data on certain database objects**
Used to obtain reference information on the main objects of the database
- **report methods**
As a rule, the response of these methods is the result of a complex analysis of database objects and contains some recommendations for eliminating / optimizing the operation of scanned objects.

## Possibilities

- scanning indexes of database tables to identify duplicates
- scanning database tables to identify unused indexes
- scanning database stored procedures and generating recommendations for their optimization
- definition of stored procedures that use previously deleted database objects (tables, columns, views)
- issuing recommendations on the need to create statistics for the columns of database tables

Information is displayed in the form of reports. A standard report consists of an introductory part (Header), main content and a final part.
- Header contains report title (title) and description/comment (description)
- The report content (reportItems) contains the result of processing the database object
- result contains a summary and general recommendations for optimizing scanned objects

```javascript
{
  "title": "string",
  "description": "string",
  "result": [
    "string"
  ],
  "reportItems": [
    {
      "reportItemStatus": 0,
      "annotation": "string",
      "query": "string"
    }
  ]
}
```
The **Content** of a report consists of a recommendation type (reportItemStatus). There are several types:
- None no recommendation
- Success object optimization possible
- Warning not enough data to form a recommendation
- Error object processing error


## Settings

This service is developed on the NET 6.0 platform. To install the service you need:
- have a locally installed web server
- DBMS MS SQL Server Express Edition 2019 or higher
- ASP.NET Core Runtime 6.0 or higher
You can publish a service using ASP.NET Core Runtime or MS Visual Studio 2022 commands. Before publishing, make sure you have sufficient rights to create a new database.
The connection to the database is made in the appsettings.json file:

```javascript
"ConnectionStrings": {
    "DefaultConnection": "Server=.; Database=DBAnalyzer; Trusted_Connection=True; MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
```

## Preparation

After installing the service and creating the database. You need to open the [config].[DataSources] table and add connection strings to the [Value] column in the format:

[Date Creation] | [name] | [value] | [Comment] |
  ------------- | ------ | ------- | -----------:|
  2023-02-02 | database name | server=.; Database=DBAnalyzer; Trusted_Connection=True; MultipleActiveResultSets=true;TrustServerCertificate=true | Your comment
 
There must be at least one entry with the data source.
 
## Requests

You need to establish a current connection. Connect to the service, the swagger with service endpoints will open. Complete

/api/1.0/configuration/datasources

which will give you the current collection of database connection strings

```javascript
[
  {
    "id": 1,
    "dateCreation": "2023-02-02T00:00:00",
    "name": "My Temp DB",
    "value": "Server=.; Database=Temp; Trusted_Connection=True; MultipleActiveResultSets=true;TrustServerCertificate=true",
    "comment": ""
  }
]
```
Copy the **id** of the connection.
Establish a current connection, make a POST request to /api/1.0/configuration/datasource. Pass the connection **id** in the request body.
If the save setup is successful, you will get an object with the text:

**Active data source set**

## Getting a report

POST requests (/api/1.0/reports) are used to generate reports. An object is passed to the request body that affects the format of the report presentation. It is an output filter.
Request example:

```javascript
{
   "showWithRecommendations": true,
   "showWithoutOptimizations": true,
   "showWithErrors": true
}
```

Response example:

```javascript
{
  "title": "Formation of a list of database indexes to optimize the execution of stored procedures",
  "description": "Contains a list of scripts for creating database objects based on analysis of stored procedure execution plans",
  "result": [
    "Errors: 0",
    "Warnings: 1",
    "Optimizations: 1"
  ],
  "reportItems": [
  {
      "reportItemStatus": 1,
      "annotation": "Procedure [dbo].[GetPointCashBoxByDateInsert], impact: 44,6622%, statistics: not found",
      "query": "CREATE NONCLUSTERED INDEX [IX_Table_DateInsert] ON [dbo].[Table] ([DateInsert])\r\nINCLUDE ([UserInsert],[DateUpdate],[UserUpdate])"
    }
    ]
}
```