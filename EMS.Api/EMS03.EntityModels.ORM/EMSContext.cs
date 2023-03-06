
using EMS.Models;
using EMS02.EntityModels.EM02.LoginBusiness;
using Microsoft.EntityFrameworkCore;


namespace EMS03.EntityModels.ORM
{
    public class EMSContext:DbContext
    {
        public EMSContext(DbContextOptions<EMSContext> options) : base(options) { }

        public EMSContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentCategory> EquipmentCategory { get; set; }
        public DbSet<EquipmentReport> EquipmentReport { get; set; }
        public DbSet<EquipmentScrap> EquipmentScrap { get; set; }
        public DbSet<EquipmentUseWithUser> EquipmentUseWithUser { get; set; }
        public DbSet<TransactionNotification> TransactionNotification { get; set; }
        public DbSet<Login> Login { get; set; }

    }
}
