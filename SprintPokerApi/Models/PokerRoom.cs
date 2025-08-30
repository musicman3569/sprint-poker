using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SprintPokerApi.Models;

public class PokerRoom : AuditableEntity
{
    public Guid PokerRoomId { get; set; } = Guid.NewGuid();
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public List<PokerPlayer> PokerPlayers { get; set; } = new();
    public List<Vote> Votes { get; set; } = new();
    public int CardSetId { get; set; }
    public CardSet CardSet { get; set; } = new();
    
    public List<PokerPlayer> CurrentPlayers { get; set; } = new();

    public PokerRoom() {}
    
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