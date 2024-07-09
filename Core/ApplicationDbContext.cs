using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ikagai.Core
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Status> Statuses { get; set; }
        public DbSet<BloodAndDerivatives> BloodAndDerivatives { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<BloodBank> BloodBanks { get; set; }
        public DbSet<DeliveryCompany> DeliveryCompanies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DonorOrder> DonorOrders { get; set; }
        public DbSet<BloodBankOrder> BloodBankOrders { get; set; }
        public DbSet<DeliveryCompanyServices> DeliveryCompanyServices { get; set; }
        public DbSet<DonationRequest> DonationRequests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
    }
}
