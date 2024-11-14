using DataLayer.Database.Schema.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace DataLayer.Database;

/// <summary>
/// Application Database Context that allows for access to all tables in the Database.
/// </summary>
public class DatabaseContext(DbContextOptions options, IConfiguration config): DbContext(options) {
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(config.GetConnectionString("Postgres"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AbstractSchemaModel).Assembly);
    }

    /// <summary>
    /// When changes are made to the DB ensure that the updated at time is set on the model.
    /// </summary>
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
        List<EntityEntry<AbstractSchemaModel>> entries = ChangeTracker
            .Entries<AbstractSchemaModel>()
            .Where(e => e.State == EntityState.Added
                || e.State == EntityState.Modified)
            .ToList();
        
        foreach (EntityEntry<AbstractSchemaModel> entityEntry in entries)
        {
            entityEntry.Entity.UpdatedAt = DateTime.UtcNow;
            entityEntry.Property(e => e.UpdatedAt).IsModified = true;
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}