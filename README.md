**This console application provides utilities in .NET that makes it easy to interact with Azure Cosmos DB.**

1. Create a new console application from Visual Studio/rider templates.
2. Add the following Nuget packages to the project:
    - Microsoft.Azure.Cosmos
3. Copy the connection string from the Azure portal--> Azure Cosmos DB account--> Keys--> Primary Connection String.
![img.png](AzCosmosDbDemo/Images/img.png)
4. Add the following code to the Program.cs file for referencing the connection string.

   **Note:-** Donot expose the connection string publicly. I'm using it here for demonstration purposes only. I will delete this connection string after the demo.
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

**Adding Items to the container**

8. Add the following code to the Program.cs file for adding items to the container
```csharp
    CosmosClient client = ConnectDatabase();
    Database database = client.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);
    
    ItemResponse<CosmosUser> item= await container.CreateItemAsync<CosmosUser>(user, new PartitionKey(user.username));
    Console.WriteLine("Item created with id: {0}", item.Resource.id);
    Console.WriteLine("Item created with StatusCode: {0}", item.StatusCode);
    Console.WriteLine("");
```
9. Run the application and check the Azure portal for the newly added items in the container. See the console logs to verify the successful addition of items to the container.

   ![img_3.png](AzCosmosDbDemo/Images/img_3.png)

10. Navigate to the Azure portal and check the data in the container.
    ![img_4.png](AzCosmosDbDemo/Images/img_4.png)

**Querying Items from the container**
1. Add the following code to the Program.cs file for querying items from the container
```csharp
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
```
2. Run the application and check the console logs to verify the successful retrieval of items from the container.

   ![img_5.png](AzCosmosDbDemo/Images/img_5.png)

**Updating Items in the container**
1. Add the following code to the Program.cs file for updating items in the container
```csharp
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
}
```
2. Invoke the UpdateUserEmail method by passing the id, username, and email of the user to be updated.
```csharp
    await UpdateUserEmail("3", "Andy", "andynew1@demo.com");
```
3. Run the application and check the console logs to verify the successful update of items in the container.

   ![img_6.png](AzCosmosDbDemo/Images/img_6.png)
4. We can also update the items in the container using the PatchItemAsync Method as shown below.
```csharp
    await container.PatchItemAsync<CosmosUser>(id, new PartitionKey(username),
         new[] { PatchOperation.Replace("/email", email) });
    Console.WriteLine("Item updated with email: {0}", email);
```

**Deleting Items in the container**
1. Add the following code to the Program.cs file for deleting items in the container
```csharp
async Task DeleteUser(string id,string username)
{
   CosmosClient client = ConnectDatabase();
    Database database = client.GetDatabase(databaseName);
    Container container = database.GetContainer(containerName);
    await container.DeleteItemAsync<CosmosUser>(id, new PartitionKey(username));
    Console.WriteLine("User Deleted with id: {0}", id);
}
```
2. Invoke the DeleteUser method by passing the id and username of the user to be deleted.
3. Run the application and check the console logs to verify the successful deletion of items in the container.

   ![img_7.png](AzCosmosDbDemo/Images/img_7.png)

4. Navigate to azure portal--> check Azure Cosmos DB account--> Data Explorer--> Select the database and container--> Click on Items to verify the deletion of items in the container.
   The record with id 3 is deleted from the container.


   ![img_8.png](AzCosmosDbDemo/Images/img_8.png)

**Creating Array of Items in the container**

1. Add the following code to the Program.cs file for creating an array of items in the container
```csharp
//Create a new class Student as below.
    public class Student
{
    public string? studentId { get; set; }
    public string? studentName { get; set; }
    public decimal price { get; set; }
    public Student(string studentid, string studentname, decimal price)
    {
        this.studentId = studentid;
        this.studentName = studentname;
        this.price = price;
    }
}
//also now modify the CosmosUser class as below to include the Student array.
public class CosmosUser
{
    public string id { get; set; }
    public string username { get; set; }
    public string designation { get; set; }
    public string email { get; set; }
    public int[] publicChapters { get; set; }
    public List<Student> students { get; set; }
    public CosmosUser()
    {
        
    }

    public CosmosUser(string username, string designation, string email, int[] publicChapters, List<Student> students)
    {
        this.id = Guid.NewGuid().ToString();
        this.username = username;
        this.designation = designation;
        this.email = email;
        this.publicChapters = publicChapters;
        this.students = students;
    }
}

```
2. We can use the same method to create a user with students.
```csharp
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
```
3. Invoke the CreateUser method by passing the user object with students.
```csharp
    CosmosUser user1 = new CosmosUser { id = "1", username = "Andrew", designation = "Developer", email = "user1@demo.com", publicChapters = new int[] { 1, 2, 3 }, students = new List<Student> { new Student("1", "John", 100), new Student("2", "Doe", 200) } };
    await CreateUser(user1);
```
4. Run the application and check the console logs to verify the successful creation of items in the container.
   ![img_9.png](AzCosmosDbDemo/Images/img_9.png)


5. Navigate to azure portal--> check Azure Cosmos DB account--> Data Explorer--> Select the database and container--> Click on Items to verify the creation of items in the container.
   The record with id 1 is created in the container.
   ![img_10.png](AzCosmosDbDemo/Images/img_10.png)

**Querying Array of Items from the container**
1. Add the following code to the Program.cs file for querying array of items from the container
```csharp
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
            Console.WriteLine("Public Chapters");
            foreach (int chapter in user.publicChapters)
            {
                Console.WriteLine(chapter);
            }
            Console.WriteLine("Students Information");
            foreach (Student student in user.students)
            {
                Console.WriteLine("Student: {0}, {1}, {2}", student.studentId, student.studentName, student.price);
            }
                
            Console.WriteLine("-----------------------------------");
        }
    }
}

```
2. Run the application and check the console logs to verify the successful retrieval of items from the container.

   ![img_11.png](AzCosmosDbDemo/Images/img_11.png)