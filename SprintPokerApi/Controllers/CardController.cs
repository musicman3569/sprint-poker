using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SprintPokerApi.Data;
using SprintPokerApi.Models;

namespace SprintPokerApi.Controllers;

[ApiController]
[Authorize]
[Route("v1/[controller]")]
public class CardController : ControllerBase
{
    private readonly ILogger<CardController> _logger;
    private readonly AppDbContext _dbContext;

    public CardController(ILogger<CardController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Card>>> GetCards()
    {
        return Ok(await _dbContext.Cards.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Card>> GetCard(int id)
    {
        var card = await _dbContext.Cards.FindAsync(id);
        if (card == null)
            return NotFound();
        return card;
    }

    [HttpPost]
    public async Task<ActionResult<Card>> CreateCard(Card card)
    {
        _dbContext.Cards.Add(card);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCard), new { id = card.CardId }, card);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCard(Card card)
    {
        _dbContext.Entry(card).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        var card = await _dbContext.Cards.FindAsync(id);
        if (card == null)
            return NotFound();
        _dbContext.Cards.Remove(card);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}