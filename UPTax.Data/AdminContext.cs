using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using UPTax.Model.Models;
using UPTax.Model.Models.Account;
using UPTax.Model.Models.UnionDetails;

namespace UPTax.Data
{
    public class AdminContext : IdentityDbContext<ApplicationUser>
    {
        public AdminContext() : base("Admin_Context")
        {
            //this.Configuration.LazyLoadingEnabled = true;
            //this.Configuration.ProxyCreationEnabled = true;
        }

        public DbSet<UnionParishad> UnionParishads { get; set; }
        public DbSet<WardInfo> WardInfos { get; set; }
        public DbSet<EducationInfo> EducationInfos { get; set; }
        public DbSet<VillageInfo> VillageInfos { get; set; }
        public DbSet<ProfessionInfo> ProfessionInfos { get; set; }
        public DbSet<SocialBenefit> SocialBenefits { get; set; }
        public DbSet<InfrastructureInfo> InfrastructureInfos { get; set; }
        public DbSet<InstituteInfo> InstituteInfos { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("Roles");
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("UserRoles");
        }

        public bool Seed(AdminContext context)
        {
            bool success = false;
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            if (!roleManager.RoleExists("Super Admin"))
            {
                success = this.CreateRole(roleManager, "Super Admin", "Union Autority");
            }
            if (!success == true) return false;
            if (!roleManager.RoleExists("Admin"))
            {
                success = this.CreateRole(roleManager, "Admin", "Edit existing records");
            }

            if (!success == true) return false;

            if (!roleManager.RoleExists("User"))
            {
                success = this.CreateRole(roleManager, "User", "Restricted to business domain activity");
            }
            if (!success) return false;


            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var user = new ApplicationUser();
            var passwordHasher = new PasswordHasher();

            user.UserName = "Admin";
            user.FullName = "ইউনিওন পরিষদ";


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                IdentityResult result = userManager.Create(user, "Up@123");
            }

            //success = this.AddUserToRole(userManager, user.Id, "Admin");
            //if (!success) return false;

            success = this.AddUserToRole(userManager, user.Id, "Super Admin");
            if (!success) return false;

            //success = this.AddUserToRole(userManager, user.Id, "User");
            //if (!success) return false;

            return true;
        }

        public bool CreateRole(ApplicationRoleManager _roleManager, string name, string description = "")
        {
            var idResult = _roleManager.Create<ApplicationRole, string>(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }

        public bool AddUserToRole(ApplicationUserManager _userManager, string userId, string roleName)
        {
            var idResult = _userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

    }

}
