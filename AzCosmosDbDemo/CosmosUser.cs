namespace AzCosmosDbDemo;

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