using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Database.Schema.Abstract;
using DataLayer.Database.Schema.GiftList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Identity;

namespace DataLayer.Database.Schema.Identity;

/// <summary>
/// This is the schema model that represents accounts in the server.
/// </summary>
[Table("Accounts")]
public class AccountSchemaModel : AbstractSchemaModel {
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public AccountRole Role { get; set; }
    public string? DisplayName { get; set; }
    public string? RefreshToken { get; set; }
    public AccessStatus Status { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    public List<GiftListSchemaModel> GiftLists { get; set; }

    public class Configuration : Configuration<AccountSchemaModel> {
        public override void Configure(EntityTypeBuilder<AccountSchemaModel> builder) {
            base.Configure(builder);
            builder.HasIndex(m => m.Email).IsUnique();
            builder.HasIndex(m => m.Username).IsUnique();

            builder.Property(m => m.Username).IsRequired();
            builder.Property(m => m.Email).IsRequired();
            builder.Property(m => m.Status)
                .IsRequired()
                .HasDefaultValue(AccessStatus.Pending);
            builder.Property(m => m.Role)
                .IsRequired()
                .HasDefaultValue(AccountRole.User);
            builder.Property(m => m.DisplayName).IsRequired(false);
            builder.Property(m => m.RefreshToken).IsRequired(false);
            builder.Property(m => m.DateOfBirth).IsRequired();
        }
    }
    
}