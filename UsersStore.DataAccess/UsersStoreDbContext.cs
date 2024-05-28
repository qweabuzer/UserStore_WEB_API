using Microsoft.EntityFrameworkCore;
using UsersStore.DataAccess.Entities;

namespace UsersStore.DataAccess
{
    public class UsersStoreDbContext : DbContext
    {
        public UsersStoreDbContext(DbContextOptions<UsersStoreDbContext> options)
            :base(options)
        {
        }
        public DbSet<UserEntity> Users { get; set; }
    }
}
