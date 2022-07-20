using JobPortal.Areas.Config.Models;
using JobPortal.Areas.Jobs.Models;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<AdminUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AdminUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        public DbSet<Job> Job { get; set; }
        public DbSet<JobCategory> JobCategory { get; set; }
        public DbSet<JobQualification> JobQualification { get; set; }
        public DbSet<JobType> JobType { get; set; }
        public DbSet<SalaryType> SalaryType { get; set; }
        public DbSet<SalaryRange> SalaryRange { get; set; }
        public DbSet<JobExperience> JobExperience { get; set; }
        public DbSet<JobShift> JobShift { get; set; }
        public DbSet<JobLevel> JobLevel { get; set; }

    }
}