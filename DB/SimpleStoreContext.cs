using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;

namespace SimpleStore.DB
{
    public class SimpleStoreContext : DbContext
    {
        public SimpleStoreContext(DbContextOptions<SimpleStoreContext> options)
            : base(options)
        {
        }

        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
