using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

public class CardSet : AuditableEntity
{
    public int CardSetId { get; set; }
    [MaxLength(36)]
    public string Name { get; set; } = string.Empty;
}