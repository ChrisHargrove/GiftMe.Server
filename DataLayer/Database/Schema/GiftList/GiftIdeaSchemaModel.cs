using System.ComponentModel.DataAnnotations.Schema;
using DataLayer.Database.Schema.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Database.Schema.GiftList;

/// <summary>
/// This schema model represents the items that get stored inside of the gift list. Each item has to e owned by a list, This means that items cannot be copied between lists or be reused.
/// </summary>
[Table(("GiftIdeas"))]
public class GiftIdeaSchemaModel : AbstractSchemaModel {
    public string Name { get; set; } = null!;
    public int? Cost { get; set; }
    public string? Link { get; set; }
    public string? Image { get; set; }
    public bool? IsPurchased { get; set; }
    
    public Guid GiftListId { get; set; }
    public GiftListSchemaModel GiftList { get; set; }

    public class Configuration : Configuration<GiftIdeaSchemaModel> {
        public override void Configure(EntityTypeBuilder<GiftIdeaSchemaModel> builder) {
            base.Configure(builder);

            builder.Property(m => m.Name).IsRequired();
            builder.Property(m => m.Cost).IsRequired(false);
            builder.Property(m => m.Link).IsRequired(false);
            builder.Property(m => m.Image).IsRequired(false);
            builder.Property(m => m.IsPurchased).IsRequired(false);

            //NOTE: Ensure that we define the relationship on the dependant model inside tha table.
            builder.HasOne<GiftListSchemaModel>(m => m.GiftList)
                .WithMany(m => m.GiftIdeas)
                .IsRequired()
                .HasForeignKey(m => m.GiftListId);
        }
    }
}