using DMAS_EXAM.Data;
using DMAS_EXAM.DTOs;
using DMAS_EXAM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMAS_EXAM.Controllers;

[ApiController]
[Route("api")]
public class AssetController : ControllerBase
{
    private readonly AppDbContext _context;

    public AssetController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Requirement 2: When admin creates a new asset -> post all information into the Asset table.
    /// POST /api/createasset
    /// </summary>
    [HttpPost("createasset")]
    public async Task<IActionResult> CreateAsset([FromBody] CreateAssetRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var asset = new Asset
        {
            AssetName = request.AssetName,
            LevelRequire = request.LevelRequire
        };

        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAssetById), new { id = asset.AssetId }, asset);
    }

    /// <summary>
    /// Utility endpoint: list all assets (useful for the client dropdown / testing).
    /// GET /api/assets
    /// </summary>
    [HttpGet("assets")]
    public async Task<IActionResult> GetAssets()
    {
        var assets = await _context.Assets.AsNoTracking().ToListAsync();
        return Ok(assets);
    }

    /// <summary>
    /// Utility endpoint: get a single asset by id.
    /// GET /api/assets/{id}
    /// </summary>
    [HttpGet("assets/{id:int}")]
    public async Task<IActionResult> GetAssetById(int id)
    {
        var asset = await _context.Assets.AsNoTracking()
            .FirstOrDefaultAsync(a => a.AssetId == id);

        if (asset is null)
        {
            return NotFound(new { message = $"Asset with id {id} was not found." });
        }

        return Ok(asset);
    }
}