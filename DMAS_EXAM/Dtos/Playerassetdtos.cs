using System.ComponentModel.DataAnnotations;

namespace DMAS_EXAM.DTOs;

/// <summary>
/// Request body for POST /api/assignasset
/// (utility endpoint so PlayerAsset data can be populated/tested;
/// not one of the 3 graded APIs but needed to demo getassetsbyplayer)
/// </summary>
public class AssignAssetRequest
{
    [Required]
    public int PlayerId { get; set; }

    [Required]
    public int AssetId { get; set; }
}

/// <summary>
/// One row of the report returned by GET /api/getassetsbyplayer
/// Matches the table shape required in the exam (No, Player name, Level, Age, Asset name)
/// </summary>
public class PlayerAssetReportDto
{
    public int No { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Age { get; set; } = string.Empty;
    public string AssetName { get; set; } = string.Empty;
}

