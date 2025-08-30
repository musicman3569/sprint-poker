using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

public class Card : AuditableEntity
{
    public int CardId { get; set; }
    public int Value { get; set; }
    [MaxLength(24)]
    public string? DisplayName { get; set; }
    public int CardSetId { get; set; }
    public CardSet CardSet { get; set; } = new();
}