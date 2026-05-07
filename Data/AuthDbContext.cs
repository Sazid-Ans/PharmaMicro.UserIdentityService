using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmaMicro.UserIdentityService.Models.Pharmacy;
using PharmaMicro.UserIdentityService.Models.Users;

namespace PharmaMicro.UserIdentityService.Data
{
    /// <summary>
    /// This class is the bridge between the application and the SQL Server.
    /// Since it inherits IdentityDbContext<ApplicationUser>, it automatically includes all identity tables.
    /// </summary>
    public class AuthDbContext:IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyStaff> PharmacyStaff { get; set; }
    }
}
