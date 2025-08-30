using System.ComponentModel.DataAnnotations;

namespace SprintPokerApi.Models;

/// <summary>
/// Represents the available voting flag options for a vote in planning poker.
/// Flag options are used to express uncertainty and the player's desired allowances
/// around their voting value for negotiating above or below their estimate.
/// </summary>
public enum VoteFlagId
{
    /// <summary>No vote flag is set.</summary>
    None = 0,
    /// <summary>Vote can be increased.</summary>
    AllowUp = 1,
    /// <summary>Vote can be decreased.</summary>
    AllowDown = 2,
    /// <summary>Vote can be both increased and decreased.</summary>
    AllowUpDown = 3,
}

/// <summary>
/// Represents a vote flag entity that defines the voting behavior and its description.
/// </summary>
public class VoteFlag
{
    /// <summary>The identifier representing the type of vote flag.</summary>
    public VoteFlagId VoteFlagId { get; set; }
    
    /// <summary>The name of the vote flag. Limited to 24 characters.</summary>
    [MaxLength(24)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>The description of the vote flag. Limited to 255 characters.</summary>
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}