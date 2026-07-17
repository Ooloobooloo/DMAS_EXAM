using DMAS_EXAM.Data;
using DMAS_EXAM.DTOs;
using DMAS_EXAM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMAS_EXAM.Controllers;

[ApiController]
[Route("api")]
public class PlayerAssetController : ControllerBase
{
    private readonly AppDbContext _context;

    public PlayerAssetController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Utility endpoint: give a player an asset. Needed so PlayerAsset has data to report on.
    /// Not one of the 3 graded APIs, but required to be able to demo requirement 3.
    /// POST /api/assignasset
    /// </summary>
    [HttpPost("assignasset")]
    public async Task<IActionResult> AssignAsset([FromBody] AssignAssetRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var playerExists = await _context.Players.AnyAsync(p => p.PlayerId == request.PlayerId);
        if (!playerExists)
        {
            return NotFound(new { message = $"Player with id {request.PlayerId} was not found." });
        }

        var assetExists = await _context.Assets.AnyAsync(a => a.AssetId == request.AssetId);
        if (!assetExists)
        {
            return NotFound(new { message = $"Asset with id {request.AssetId} was not found." });
        }

        var alreadyOwned = await _context.PlayerAssets.AnyAsync(pa =>
            pa.PlayerId == request.PlayerId && pa.AssetId == request.AssetId);
        if (alreadyOwned)
        {
            return Conflict(new { message = "This player already owns this asset." });
        }

        var playerAsset = new PlayerAsset
        {
            PlayerId = request.PlayerId,
            AssetId = request.AssetId
        };

        _context.PlayerAssets.Add(playerAsset);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Asset assigned to player successfully." });
    }

    /// <summary>
    /// Requirement 3: Report of assets owned by players.
    /// GET /api/getassetsbyplayer
    /// Optional query string: ?playerId=1  -> filter the report to a single player.
    /// </summary>
    [HttpGet("getassetsbyplayer")]
    public async Task<IActionResult> GetAssetsByPlayer([FromQuery] int? playerId = null)
    {
        var query = _context.PlayerAssets
            .AsNoTracking()
            .Include(pa => pa.Player)
            .Include(pa => pa.Asset)
            .AsQueryable();

        if (playerId.HasValue)
        {
            query = query.Where(pa => pa.PlayerId == playerId.Value);
        }

        var rows = await query
            .OrderBy(pa => pa.PlayerId)
            .Select(pa => new
            {
                PlayerName = pa.Player!.PlayerName,
                Level = pa.Player.Level,
                Age = pa.Player.Age,
                AssetName = pa.Asset!.AssetName
            })
            .ToListAsync();

        var report = rows.Select((row, index) => new PlayerAssetReportDto
        {
            No = index + 1,
            PlayerName = row.PlayerName,
            Level = row.Level,
            Age = row.Age,
            AssetName = row.AssetName
        }).ToList();

        return Ok(report);
    }
}