using System.ComponentModel.DataAnnotations;

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

    public PokerRoom() {}
}