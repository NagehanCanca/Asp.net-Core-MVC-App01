using Microsoft.EntityFrameworkCore;

namespace NorthwindModel
{
    public class NorthwindDbContext : DbContext
    {
        // Diğer DbSet tanımlamaları...

        public DbSet<Employee> Employees { get; set; }
    }

}