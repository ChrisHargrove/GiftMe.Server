using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Database.Schema.Abstract;
using DataLayer.Database.Schema.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Database.Schema.GiftList;

/// <summary>
/// This schema model represents the GiftList inside of the system and how it related to accounts and the ability to store items inside of the list itself.
/// </summary>
[Table("GiftLists")]
public class GiftListSchemaModel : AbstractSchemaModel {
    public string Name { get; set; }
    public DateTime Date { get; set; }
    
    public Guid AccountId { get; set; }
    public AccountSchemaModel Account { get; set; }
    
    public List<GiftIdeaSchemaModel> GiftIdeas { get; set; }

    public class Configuration : Configuration<GiftListSchemaModel> {
        public override void Configure(EntityTypeBuilder<GiftListSchemaModel> builder) {
            base.Configure(builder);

            builder.Property(m => m.Name).IsRequired();
            builder.Property(m => m.Date).IsRequired().HasConversion(DateTimeConverter);
            
            //NOTE: Have to ensure that the relationship of the model to the account mode is specified on the dependant.
            builder.HasOne<AccountSchemaModel>(m => m.Account)
                .WithMany(m => m.GiftLists)
                .IsRequired()
                .HasForeignKey(m => m.AccountId);
        }
    }
}