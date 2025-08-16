namespace Application.Models.Response.People;

public class Person
{
    public required Guid RunnerId { get; set; }
    public required string NickName { get; set; }
}