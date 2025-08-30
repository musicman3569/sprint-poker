using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

/// <summary>
/// Represents a player in the planning poker session.
/// Inherits audit properties from AuditableEntity.
/// </summary>
public class PokerPlayer : AuditableEntity
{
    /// <summary>The unique identifier for the poker player.</summary>
    public Guid PokerPlayerId { get; set; } = Guid.NewGuid();

    /// <summary>The email address of the player. Limited to 256 characters.</summary>
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    /// <summary>The display name of the player shown during poker sessions. Limited to 256 characters.</summary>
    [MaxLength(256)]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>The identifier of the poker room where the player is currently present.</summary>
    public Guid? PokerRoomCurrentId { get; set; }

    /// <summary>The poker room where the player is currently present.</summary>
    public PokerRoom? PokerRoomCurrent { get; set; }

    /// <summary>The list of all poker rooms associated with this player.</summary>
    public List<PokerRoom> PokerRooms { get; set; } = new();

    /// <summary>
    /// Additional manual configuration of model indices, constraints, and relationships.
    /// </summary>
    public class Configuration : IEntityTypeConfiguration<PokerPlayer>
    {
        public void Configure(EntityTypeBuilder<PokerPlayer> builder)
        {
            builder.HasIndex(p => p.Email).IsUnique();

            builder
                .HasOne(player => player.PokerRoomCurrent)
                .WithMany(room => room.CurrentPlayers)
                .HasForeignKey(player => player.PokerRoomCurrentId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}