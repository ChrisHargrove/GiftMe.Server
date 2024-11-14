using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Database.Schema.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Identity;

namespace DataLayer.Database.Schema.Identity;

/// <summary>
/// This schema model represents both first time access requests to the system, and also when an accounts access has been modified by an admin.
/// <para>
/// This schema requires an account but the relationship is one way, so an account does not require an account accessor.
/// </para>
/// </summary>
[Table("AccountAccessors")]
public class AccountAccessorSchemaModel: AbstractSchemaModel {
    public AccessStatus Status { get; set; }
    
    public Guid AccountId { get; set; }
    public AccountSchemaModel Account { get; set; }

    public class Configuration : Configuration<AccountAccessorSchemaModel> {
        public override void Configure(EntityTypeBuilder<AccountAccessorSchemaModel> builder) {
            base.Configure(builder);
            builder.Property(m => m.Status).IsRequired();
            builder.HasOne<AccountSchemaModel>(m => m.Account)
                .WithOne()
                .IsRequired()
                .HasForeignKey<AccountAccessorSchemaModel>(m => m.AccountId);
        }
    }
}