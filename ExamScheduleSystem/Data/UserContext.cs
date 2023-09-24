using ExamScheduleSystem.DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyApiNetCore6.Data
{
    public class UserContext : IdentityDbContext<ApplicationUser>
    {
        public UserContext(DbContextOptions<UserContext> opt) : base(opt)
        {

        }

        #region DbSet
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa khóa chính cho ApplicationUser (nếu cần)
            modelBuilder.Entity<ApplicationUser>()
                .HasKey(u => u.Id);

            // Các định nghĩa khác cho mô hình cơ sở dữ liệu
        }
    }

}