using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SprintPokerApi.Models;

/// <summary>
/// Represents a vote cast by a player in a planning poker room.
/// Each vote links a player's card selection to a specific poker room.
/// </summary>
public class Vote
{
    /// <summary>The unique identifier for the vote.</summary>
    public Guid VoteId { get; set; } = Guid.NewGuid();

    /// <summary>The identifier of the selected card for this vote.</summary>
    public int CardId { get; set; }
    
    /// <summary>The card selected for this vote.</summary>
    public Card Card { get; set; } = new();

    /// <summary>The identifier of the player who cast this vote.</summary>
    public Guid PokerPlayerId { get; set; }
    
    /// <summary>The player who cast this vote.</summary>
    public PokerPlayer PokerPlayer { get; set; } = new();

    /// <summary>The identifier of the poker room where this vote was cast.</summary>
    public Guid PokerRoomId { get; set; }
    
    /// <summary>The poker room where this vote was cast.</summary>
    public PokerRoom PokerRoom { get; set; } = new();

    /// <summary>The identifier of the vote flag status.</summary>
    public VoteFlagId VoteFlagId { get; set; } = VoteFlagId.None;
    
    /// <summary>The vote flag status for this vote.</summary>
    public VoteFlag VoteFlag { get; set; } = new();

    /// <summary>
    /// Additional manual configuration of model indices, constraints, and relationships.
    /// </summary>
    public class Configuration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(v => new { v.PokerPlayerId, v.PokerRoomId }).IsUnique();
        }
    }
}