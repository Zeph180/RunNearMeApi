using Application.Models.Response.People;

namespace Application.Interfaces;

public interface IPeople
{
    Task<List<Person>> GetPeople(Guid RunnerId);
}