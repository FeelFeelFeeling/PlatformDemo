namespace Repostory.Power.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Power : DbContext
    {
        public Power()
            : base("data source=.;initial catalog=PetRescue;persist security info=True;user id=sa;password=123456;MultipleActiveResultSets=True")
        {
        }

        public virtual DbSet<SystemUserInfo> SystemUserInfoes { get; set; }
        public virtual DbSet<SystemUserMenu> SystemUserMenus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.SystemUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.LoginId)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.WXCode)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.NOCode)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.CreateUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .Property(e => e.ModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserInfo>()
                .HasMany(e => e.SystemUserMenus)
                .WithRequired(e => e.SystemUserInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SystemUserMenu>()
                .Property(e => e.SystemUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUserMenu>()
                .Property(e => e.MenuIdentity)
                .IsUnicode(false);
        }
    }
}
