using Microsoft.EntityFrameworkCore;
using SprintPokerApi.Models;

namespace SprintPokerApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Card> Cards { get; set; }
    public DbSet<CardSet> CardSets { get; set; }
    public DbSet<PokerPlayer> PokerPlayers { get; set; }
    public DbSet<PokerRoom> PokerRooms { get; set; }
    public DbSet<Vote> Votes { get; set; }
    
    
    /// <summary>
    /// Saves all changes made in this context to the database with automatic audit property handling.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains
    /// the number of state entries written to the database.
    /// </returns>
    /// <remarks>
    /// This method automatically sets audit properties (CreatedAt, CreatedBy, ModifiedAt, ModifiedBy) for entities
    /// that inherit from AuditableEntity. For new entities, CreatedAt, CreatedBy, ModifiedAt, and ModifiedBy
    /// are set to current UTC time and user. For modified entities, only ModifiedAt and ModifiedBy are updated.
    /// </remarks>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Get all the entities that inherit from AuditableEntity
        // and have a state of Added or Modified
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is AuditableEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified
            ));

        // For each entity we will set the Audit properties
        foreach (var entityEntry in entries)
        {
            // If the entity state is Added let's set
            // the CreatedAt and CreatedBy properties
            if (entityEntry.State == EntityState.Added)
            {
                ((AuditableEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                // TODO: Get username from the token
                /* ((AuditableEntity)entityEntry.Entity).CreatedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp"; */
            }
            else
            {
                // If the state is Modified then we don't want
                // to modify the CreatedAt and CreatedBy properties
                // so we set their state as IsModified to false
                Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
            }

            // In any case we always want to set the properties
            // ModifiedAt and ModifiedBy
            ((AuditableEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
            // TODO: Get username from the token
            /* ((AuditableEntity)entityEntry.Entity).ModifiedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp"; */
        }

        // After we set all the needed properties
        // we call the base implementation of SaveChangesAsync
        // to actually save our entities in the database
        return await base.SaveChangesAsync(cancellationToken);
    }
}