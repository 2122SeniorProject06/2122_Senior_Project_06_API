using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using _2122_Senior_Project_06.Types;

namespace _2122_Senior_Project_06.DataContext
{
    /// <summary>
    /// Class to access database. Written by Sarah.
    /// </summary>
    public class ShardContext : DbContext
    {
        public ShardContext(DbContextOptions<ShardContext> options)
            : base(options)
        {

        }

       public DbSet<JournalEntry> Journals { get; set; } = null!;

    }
}