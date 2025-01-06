using AzCosmosDbDemo;
using Microsoft.Azure.Cosmos;

string databaseName = "myappdb59";
string containerName = "users";
string partitionKey = "/username";
CosmosClient client = ConnectDatabase();

//Database Creation
Database database = CreateDatabase(databaseName);
Console.WriteLine("Database created with id: {0}", database.Id);

//Container Creation
Container container = CreateContainer(containerName, partitionKey);
Console.WriteLine("Container created with id: {0}", container.Id);

//Adding items to container
CosmosUser user1 = new CosmosUser { id = "1", username = "Andrew", designation = "Developer", email = "user1@demo.com" };
CosmosUser user2 = new CosmosUser { id = "2", username = "Michael", designation = "Senior Developer", email = "Michael@demo.com" };
CosmosUser user3 = new CosmosUser { id = "3", username = "Andy", designation = "Senior Developer", email = "Andy@demo.com" };

await CreateUser(user1);
await CreateUser(user2);
await CreateUser(user3);

async Task CreateUser(CosmosUser user)
{
    CosmosClient client = ConnectDatabase();
    Database database = client.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);
    
    ItemResponse<CosmosUser> item= await container.CreateItemAsync<CosmosUser>(user, new PartitionKey(user.username));
    Console.WriteLine("Item created with id: {0}", item.Resource.id);
    Console.WriteLine("Item created with StatusCode: {0}", item.StatusCode);
    Console.WriteLine("");
}


CosmosClient ConnectDatabase()
{
    string connectionString="AccountEndpoint=https://myazcosmosdbinstance.documents.azure.com:443/;AccountKey=e2qYKqY9NrS95G5YSgRAJGkh79VQN1DH3QGQ6OmpFfezF5dW9TldvvJgj90Z9oKAqEtXJaC3EXY7ACDbe417Dw==;";
    return new CosmosClient(connectionString);
}

Database CreateDatabase(string databaseName)
{
    return client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
}
Container CreateContainer(string containerName, string partitionKey)
{
    return database.CreateContainerIfNotExistsAsync(containerName, partitionKey).Result;
}