using DMAS_EXAM.Data;
using DMAS_EXAM.DTOs;
using DMAS_EXAM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMAS_EXAM.Controllers;

[ApiController]
[Route("api")]
public class PlayerController : ControllerBase
{
    private readonly AppDbContext _context;

    public PlayerController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Requirement 1: When a user registers a game -> post all information into the Player table.
    /// POST /api/registerplayer
    /// </summary>
    [HttpPost("registerplayer")]
    public async Task<IActionResult> RegisterPlayer([FromBody] RegisterPlayerRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var player = new Player
        {
            PlayerName = request.PlayerName,
            FullName = request.FullName,
            Age = request.Age,
            Level = request.Level,
            Email = request.Email
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlayerById), new { id = player.PlayerId }, player);
    }

    /// <summary>
    /// Utility endpoint: list all players (useful for the client dropdown / testing).
    /// GET /api/players
    /// </summary>
    [HttpGet("players")]
    public async Task<IActionResult> GetPlayers()
    {
        var players = await _context.Players.AsNoTracking().ToListAsync();
        return Ok(players);
    }

    /// <summary>
    /// Utility endpoint: get a single player by id.
    /// GET /api/players/{id}
    /// </summary>
    [HttpGet("players/{id:int}")]
    public async Task<IActionResult> GetPlayerById(int id)
    {
        var player = await _context.Players.AsNoTracking()
            .FirstOrDefaultAsync(p => p.PlayerId == id);

        if (player is null)
        {
            return NotFound(new { message = $"Player with id {id} was not found." });
        }

        return Ok(player);
    }
}