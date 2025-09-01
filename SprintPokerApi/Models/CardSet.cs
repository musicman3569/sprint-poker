using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

/// <summary>
/// Represents a collection of planning poker cards that can be used for story point estimation.
/// This can be used to create different sprint point schemes such as fibonacci or t-shirt sizes.
/// Inherits audit properties from AuditableEntity.
/// </summary>
public class CardSet : AuditableEntity
{
    /// <summary>The unique identifier for the card set.</summary>
    public int CardSetId { get; set; }
    
    /// <summary>The name of the card set. Limited to 36 characters.</summary>
    [MaxLength(36)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>The list of cards in this card set.</summary>
    public List<Card> Cards { get; set; } = new();
}