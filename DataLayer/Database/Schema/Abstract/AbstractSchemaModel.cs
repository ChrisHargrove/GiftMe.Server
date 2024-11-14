using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer.Database.Schema.Abstract;

/// <summary>
/// The base schema model for every model that goes into the database.
/// Contains the basic properties of all models such as: Id, CreatedAt, UpdatedAt and DeletedAt.
/// </summary>
public abstract class AbstractSchemaModel: ISchemaModel {
    /// <summary>
    /// Primary key of the model in the database.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// When was this model initially created?
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// When was this model last updated?
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// When was this model marked as deleted?
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// This class is to enable EF Core CLI to automatically create database migrations when changes
    /// are made.
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    public class Configuration<TBase> : IEntityTypeConfiguration<TBase>
        where TBase : AbstractSchemaModel {

        /// <summary>
        /// This is to ensure that we make all DateTime fields UTC as standard.
        /// </summary>
        protected ValueConverter<DateTime?, DateTime?> DateTimeConverter { get; } 
            = new (v => v, v => v == null 
                ? v 
                : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

        /// <summary>
        /// This method handles all the configuration for the schema model.
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<TBase> builder) {
            //We ensure that every model added to the database automatically gets a GUID assigned.
            builder.Property(m => m.Id).HasDefaultValueSql("gen_random_uuid()");
            //We also ensure that every model gets the CreatedAt and ModifiedAt set to the current time upon creation.
            builder.Property(m => m.CreatedAt).HasDefaultValueSql("now()").HasConversion(DateTimeConverter);
            builder.Property(m => m.UpdatedAt).HasDefaultValueSql("now()").HasConversion(DateTimeConverter);
            builder.Property(m => m.DeletedAt).HasConversion(DateTimeConverter);
        }
    }
}