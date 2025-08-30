namespace SprintPokerApi.Models;

public class AuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    public Guid CreatedBy { get; set; }
    public Guid ModifiedBy { get; set; }
}