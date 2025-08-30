using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SprintPokerApi.Models;

/// <summary>
/// Represents a planning poker room where players can participate in story point estimation sessions.
/// Inherits audit properties from AuditableEntity.
/// </summary>
public class PokerRoom : AuditableEntity
{
    /// <summary>The unique identifier for the poker room.</summary>
    public Guid PokerRoomId { get; set; } = Guid.NewGuid();

    /// <summary>The name of the poker room. Limited to 256 characters.</summary>
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    /// <summary>The list of all players that have been associated with this room.</summary>
    public List<PokerPlayer> PokerPlayers { get; set; } = new();

    /// <summary>The list of votes cast in this poker room.</summary>
    public List<Vote> Votes { get; set; } = new();

    /// <summary>The identifier of the card set used in this poker room.</summary>
    public int CardSetId { get; set; }

    /// <summary>The card set used for voting in this poker room.</summary>
    public CardSet CardSet { get; set; } = new();

    /// <summary>The list of players currently present in the room.</summary>
    public List<PokerPlayer> CurrentPlayers { get; set; } = new();
    
    /// <summary>
    /// Additional manual configuration of model indices, constraints, and relationships.
    /// </summary>
    public class Configuration : IEntityTypeConfiguration<PokerRoom>
    {
        public void Configure(EntityTypeBuilder<PokerRoom> builder)
        {
            // One-to-many: PokerRoom -> CurrentPlayers <- PokerPlayer.PokerRoomCurrent
            builder
                .HasMany(room => room.CurrentPlayers)
                .WithOne(player => player.PokerRoomCurrent)
                .HasForeignKey(player => player.PokerRoomCurrentId)
                .OnDelete(DeleteBehavior.SetNull); // FK is nullable

            // Many-to-many: PokerRoom <-> PokerPlayers
            builder
                .HasMany(room => room.PokerPlayers)
                .WithMany(player => player.PokerRooms)
                .UsingEntity(join => join.ToTable("PokerPlayerRooms"));
        }
    }

}