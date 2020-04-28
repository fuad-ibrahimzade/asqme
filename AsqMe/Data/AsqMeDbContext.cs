using AsqMe.Data.Models;
using AsqMe.Data.Models.Interfaces;
using AsqMe.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsqMe.Data
{
    public class AsqMeDbContext : IdentityDbContext<ApplicationUser>
    {
        public AsqMeDbContext(DbContextOptions options) : base(options)
        {
            if ((this.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                MigrateDatabse("Initial");
        }

        public static Dictionary<string, int> SeededMaxIds { get; set; }
        public static string HerokuPostgreSqlConnectionString { get; set; }
        public static string LocalSQLServerConnectionString { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((IEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((IEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            var applicationUsers = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ApplicationUser && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));
            foreach (var applicationUser in applicationUsers)
            {
                ((ApplicationUser)applicationUser.Entity).UpdatedDate = DateTime.Now;

                if (applicationUser.State == EntityState.Added)
                {
                    ((ApplicationUser)applicationUser.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            if (AsqMeDbContext.HerokuPostgreSqlConnectionString != null)
            {
                builder.HasDefaultSchema("public");//for postgre sql
                TablesToLowerCase(builder);
            }
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Answer>()
                .HasOne(item => item.Question)
                .WithMany()
                .HasForeignKey(item => item.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Earning>()
                .HasOne(item => item.ApplicationUser)
                .WithMany()
                .HasForeignKey(item => item.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Question>()
                .HasOne(item => item.Category)
                .WithMany()
                .HasForeignKey(item => item.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Question>()
                .HasOne(item => item.ApplicationUser)
                .WithMany()
                .HasForeignKey(item => item.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<QuestionTag>()
    .           HasKey(bc => new { bc.QuestionId, bc.TagId });
            builder.Entity<QuestionTag>()
                .HasOne(item => item.Question)
                .WithMany(item => item.QuestionTags)
                .HasForeignKey(item => item.QuestionId);
            builder.Entity<QuestionTag>()
                .HasOne(item => item.Tag)
                .WithMany(item => item.QuestionTags)
                .HasForeignKey(item => item.TagId);
            builder.Entity<Earning>()
                .Property(c => c.EarningsType)
                .HasConversion<string>();

            //builder.Entity<Comment>()
            //.Property(e => e.child_comment_ids)
            //.HasConversion(
            //    v => string.Join(',', v),
            //    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList<string>());
            //builder.Entity<CustomAnalytics>()
            //.Property(e => e.analytics_data)
            //.HasConversion(
            //    v => v.ToString(),
            //    v => JObject.Parse(v));
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (AsqMeDbContext.HerokuPostgreSqlConnectionString != null)
        //        optionsBuilder.UseNpgsql(HerokuPostgreSqlConnectionString);
        //    else
        //        optionsBuilder.UseSqlServer(AsqMeDbContext.LocalSQLServerConnectionString);
        //}
        public DbSet<Question> Question{ get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Earning> Earning { get; set; }

        public void MigrateDatabse(string targetMigration=null)
        {
            if (targetMigration != null)
                this.GetInfrastructure().GetService<IMigrator>().Migrate(targetMigration);
            else
                this.Database.Migrate();
        }

        public void TablesToLowerCase(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().ToLower());

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToLower());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToLower());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToLower());
                }
            }
        }
    }
}
