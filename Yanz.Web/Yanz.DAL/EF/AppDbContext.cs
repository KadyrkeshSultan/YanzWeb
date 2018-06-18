using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yanz.DAL.Entities;

namespace Yanz.DAL.EF
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<QuestionSet> QuestionSets { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<ModerMsg> ModerMsgs { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<QuestionSet>()
                .HasOne(q => q.Folder)
                .WithMany(f => f.QuestionSets)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Question>()
                .HasOne(q => q.QuestionSet)
                .WithMany(s => s.Questions)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Choice>()
                .HasOne(c => c.Question)
                .WithMany(q => q.Choices)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Question>()
                .HasOne(q => q.Set)
                .WithMany(s => s.Questions)
                .OnDelete(DeleteBehavior.Cascade);
            //builder.Ignore<Item>();
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
