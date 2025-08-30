using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

public class PokerPlayer : AuditableEntity
{
    public Guid PokerPlayerId { get; set; } = Guid.NewGuid();
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;
    [MaxLength(256)]
    public string DisplayName { get; set; } = string.Empty;
    public Guid? PokerRoomCurrentId { get; set; }
    public PokerRoom? PokerRoomCurrent { get; set; }
    public List<PokerRoom> PokerRooms { get; set; } = new();

    public class Configuration : IEntityTypeConfiguration<PokerPlayer>
    {
        public void Configure(EntityTypeBuilder<PokerPlayer> builder)
        {
            builder.HasIndex(p => p.Email).IsUnique();
        }
    }
}