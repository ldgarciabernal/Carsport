namespace Carsport.Domain.Models
{
    using Common.Models;
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {

        }

        public DbSet<Common.Models.Station> Stations { get; set; }

        public DbSet<Common.Models.Bycicle> Bycicles { get; set; }
        
        public DbSet<Common.Models.Notification> Notifications { get; set; }

        public DbSet<Common.Models.Route> Routes { get; set; }
        
        public DbSet<Common.Models.City> Cities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Route>()
                .HasRequired(r => r.Origin)
                .WithMany(c => c.Origins)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasRequired(r => r.Destiny)
                .WithMany(c => c.Destinies)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<Carsport.Common.Models.Conversation> Conversations { get; set; }

        public System.Data.Entity.DbSet<Carsport.Common.Models.Message> Messages { get; set; }

        public System.Data.Entity.DbSet<Carsport.Common.Models.MailTemplate> MailTemplates { get; set; }
    }
}
