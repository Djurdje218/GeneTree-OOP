/*using GeneTree.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeneTree.DAL.Data
{
    public class TreeContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public TreeContext(DbContextOptions<TreeContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(p => p.Id);

            // Simplified many-to-many relationship between parents and children
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Children)
                .WithMany(p => p.Parents)
                .UsingEntity<Dictionary<string, object>>(
                    "PersonRelationships",
                    r => r.HasOne<Person>().WithMany().HasForeignKey("ChildId"),
                    l => l.HasOne<Person>().WithMany().HasForeignKey("ParentId")
                );
        }
    }
}
*/