using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SprintPokerApi.Data;
using SprintPokerApi.Models;

namespace SprintPokerApi.Controllers;

[ApiController]
[Authorize]
[Route("v1/[controller]")]
public class CardSetController : ControllerBase
{
    private readonly ILogger<CardSetController> _logger;
    private readonly AppDbContext _dbContext;

    public CardSetController(ILogger<CardSetController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardSet>>> GetCardSets()
    {
        return Ok(await _dbContext.CardSets.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CardSet>> GetCardSet(int id)
    {
        var cardSet = await _dbContext.CardSets.FindAsync(id);
        if (cardSet == null)
            return NotFound();
        return cardSet;
    }

    [HttpPost]
    public async Task<ActionResult<CardSet>> CreateCardSet(CardSet cardSet)
    {
        _dbContext.CardSets.Add(cardSet);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCardSet), new { id = cardSet.CardSetId }, cardSet);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCardSet(CardSet cardSet)
    {
        _dbContext.Entry(cardSet).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return Ok(cardSet);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        var cardSet = await _dbContext.CardSets.FindAsync(id);
        if (cardSet == null)
            return NotFound();
        _dbContext.CardSets.Remove(cardSet);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}