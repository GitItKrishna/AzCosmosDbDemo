using AzCosmosDbDemo;
using Microsoft.Azure.Cosmos;

string databaseName = "myappdb59";
string containerName = "users";
string partitionKey = "/username";
CosmosClient client = ConnectDatabase();

//Database Creation
//Database database = CreateDatabase(databaseName);
// Console.WriteLine("Database created with id: {0}", database.Id);

//Container Creation
// Container container = CreateContainer(containerName, partitionKey);
// Console.WriteLine("Container created with id: {0}", container.Id);

//Adding items to container
CosmosUser user1 = new CosmosUser { id = "1", username = "Andrew", designation = "Developer", email = "user1@demo.com", publicChapters = new int[] { 1, 2, 3 }, students = new List<Student> { new Student("1", "John", 100), new Student("2", "Doe", 200) } };
CosmosUser user2 = new CosmosUser { id = "2", username = "Michael", designation = "Senior Developer", email = "Michael@demo.com" };
CosmosUser user3 = new CosmosUser { id = "3", username = "Andy", designation = "Senior Developer", email = "Andy@demo.com" };

await CreateUser(user1);
// await CreateUser(user2);
// await CreateUser(user3);

// await 
//await DisplayUsers();
//await UpdateUserEmail("3", "Andy", "andynew1@demo.com");
//await DeleteUser("3", "Andy");

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
//Update item in container
async Task UpdateUserEmail(string id,string username, string email)
{
    CosmosClient client = ConnectDatabase();
    Database database = client.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);
    
    ItemResponse<CosmosUser> item = await container.ReadItemAsync<CosmosUser>(id, new PartitionKey(username));
    CosmosUser user = item.Resource;
    user.email = email;
    item = await container.ReplaceItemAsync<CosmosUser>(user, id, new PartitionKey(username));
    Console.WriteLine("Item updated with id: {0}", item.Resource.id);
    Console.WriteLine("Item updated with StatusCode: {0}", item.StatusCode);

    // await container.PatchItemAsync<CosmosUser>(id, new PartitionKey(username),
    //     new[] { PatchOperation.Replace("/email", email) });
    // Console.WriteLine("Item updated with email: {0}", email);
}
//Delete item in container
async Task DeleteUser(string id,string username)
{
    CosmosClient client = ConnectDatabase();
    Database database = client.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);
    await container.DeleteItemAsync<CosmosUser>(id, new PartitionKey(username));
    Console.WriteLine("User Deleted with id: {0}", id);
}

//Querying items from container
async Task DisplayUsers()
{
    CosmosClient client = ConnectDatabase();
    Database database = client.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);
    
    QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM users");
    FeedIterator<CosmosUser> resultSet = container.GetItemQueryIterator<CosmosUser>(queryDefinition);
    
    while (resultSet.HasMoreResults)
    {
        FeedResponse<CosmosUser> response = await resultSet.ReadNextAsync();
        foreach (CosmosUser user in response)
        {
            Console.WriteLine("User: {0}, {1}, {2}, {3}", user.id, user.username, user.designation, user.email);
            Console.WriteLine("-----------------------------------");
        }
    }
}

CosmosClient ConnectDatabase()
{
    string connectionString="";
    return new CosmosClient(connectionString);
}

// Database CreateDatabase(string databaseName)
// {
//     return client.CreateDatabaseIfNotExistsAsync(databaseName).Result;
// }
// Container CreateContainer(string containerName, string partitionKey)
// {
//     return database.CreateContainerIfNotExistsAsync(containerName, partitionKey).Result;
// }