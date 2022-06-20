using AccountMovement.Entities;
using Microsoft.EntityFrameworkCore;
using AccountMovement.Interfaces;

namespace AccountMovement.Context;

public class ApiDatabase : DbContext, IApiDatabase
{
    public DbSet<Client> Client { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Movement> Movement { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=192.168.0.105,1433;Database=account_movement;user id=jonas;password=12345;");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}