using Application.Middlewares.ErrorHandling;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Persistence;

namespace Repository.Repositories.Helpers;

public class DbHelpers
{
    private readonly AppDbContext  _dbContext;

    public DbHelpers(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    


}