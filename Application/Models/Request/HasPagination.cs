namespace Application.Models.Request;

public class HasPagination
{
    public required Guid RunnerId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}