namespace SprintPokerApi.Models;

/// <summary>
/// Base class for entities that require audit information.
/// Provides properties for tracking creation and modification timestamps and user information.
/// </summary>
public class AuditableEntity
{
    /// <summary>
    /// The date and time when the entity was created.
    /// Defaults to the current date and time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// The date and time when the entity was last modified.
    /// Defaults to the current date and time.
    /// </summary>
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// The unique identifier of the user who created the entity.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// The unique identifier of the user who last modified the entity.
    /// </summary>
    public Guid ModifiedBy { get; set; }
}