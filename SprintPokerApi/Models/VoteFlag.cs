using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

public enum VoteFlagId
{
    None = 0,
    AllowUp = 1,
    AllowDown = 2,
    AllowUpDown = 3,
}

public class VoteFlag
{
    public VoteFlagId VoteFlagId { get; set; }
    [MaxLength(24)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}