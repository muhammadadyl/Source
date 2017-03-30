using Journals.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Journals.Data
{
    public class JournalsContext : DbContext
    {
        public JournalsContext()
            : base("name=JournalsDB")
        {
        }

        public DbSet<Journal> Journals { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journal>().ToTable("Journals");
            modelBuilder.Entity<Subscription>().ToTable("Subscriptions");
            modelBuilder.Entity<Subscription>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Journal>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}