**This console application provides utilities in .NET that makes it easy to interact with Azure Cosmos DB.**

1. Create a new console application from Visual Studio/rider templates.
2. Add the following Nuget packages to the project:
    - Microsoft.Azure.Cosmos
3. Copy the connection string from the Azure portal--> Azure Cosmos DB account--> Keys--> Primary Connection String.
![img.png](AzCosmosDbDemo/Images/img.png)
4. Add the following code to the Program.cs file for referencing the connection string.
```csharp
string connectionString="AccountEndpoint=https://myazcosmosdbinstance.documents.azure.com:443/;AccountKey=e2qYKqY9NrS95G5YSgRAJGkh79VQN1DH3QGQ6OmpFfezF5dW9TldvvJgj90Z9oKAqEtXJaC3EXY7ACDbe417Dw==;";

```
5. Add the following code to the Program.cs file for creating a new database 
```csharp
CosmosClient client = new CosmosClient(connectionString);
Database database = await client.CreateDatabaseIfNotExistsAsync("myappdb59");
Console.WriteLine("Database created with id: {0}", database.Id);
```
6. Add the following code to the Program.cs file for creating a new container
```csharp
string partitionKey = "/username";
Container container = await database.CreateContainerIfNotExistsAsync("users", partitionKey);
Console.WriteLine("Container created with id: {0}", container.Id);
```
7. Run the application and check the Azure portal for the newly created database and container. See the console logs to verify the successful creation of database and container.

![img_1.png](AzCosmosDbDemo/Images/img_1.png)

![img_2.png](AzCosmosDbDemo/Images/img_2.png)
