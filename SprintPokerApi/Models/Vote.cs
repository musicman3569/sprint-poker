using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SprintPokerApi.Models;

public class Vote
{
    public Guid VoteId { get; set; } = Guid.NewGuid();
    public int CardId { get; set; }
    public Card Card { get; set; } = new();
    public Guid PokerPlayerId { get; set; }
    public PokerPlayer PokerPlayer { get; set; } = new();
    public Guid PokerRoomId { get; set; }
    public PokerRoom PokerRoom { get; set; } = new();

    public class Configuration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasIndex(v => new { v.PokerPlayerId, v.PokerRoomId }).IsUnique();
        }
    }
}