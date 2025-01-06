using Microsoft.Azure.Cosmos;

string connectionString="AccountEndpoint=https://myazcosmosdbinstance.documents.azure.com:443/;AccountKey=e2qYKqY9NrS95G5YSgRAJGkh79VQN1DH3QGQ6OmpFfezF5dW9TldvvJgj90Z9oKAqEtXJaC3EXY7ACDbe417Dw==;";
CosmosClient client = new CosmosClient(connectionString);
Database database = await client.CreateDatabaseIfNotExistsAsync("myappdb59");
Console.WriteLine("Database created with id: {0}", database.Id);

string partitionKey = "/username";
Container container = await database.CreateContainerIfNotExistsAsync("users", partitionKey);
Console.WriteLine("Container created with id: {0}", container.Id);