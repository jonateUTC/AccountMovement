using Microsoft.EntityFrameworkCore;
using AccountMovement.Entities;

namespace AccountMovement.Interfaces
{
    public interface IApiDatabase
    {
        DbSet<Account> Account { get; set; }
        DbSet<Client> Client { get; set; }
        DbSet<Movement> Movement { get; set; }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}