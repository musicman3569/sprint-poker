using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

/// <summary>
/// Represents a planning poker card entity with its value and display properties.
/// A card belongs to a card set, and has the value that will be used for story point estimation.
/// Inherits audit properties from AuditableEntity.
/// </summary>
public class Card : AuditableEntity
{
    /// <summary>The unique identifier for the card.</summary>
    public int CardId { get; set; }
    
    /// <summary>The numerical value of the card used for story point estimation.</summary>
    public int Value { get; set; }
    
    /// <summary>The display name of the card. Limited to 24 characters.</summary>
    [MaxLength(24)]
    public string? DisplayName { get; set; }
    
    /// <summary>The identifier of the card set this card belongs to.</summary>
    public int CardSetId { get; set; }
    
    /// <summary>The associated card set that contains this card.</summary>
    public CardSet CardSet { get; set; } = new();
}