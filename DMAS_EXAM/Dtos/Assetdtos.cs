using System.ComponentModel.DataAnnotations;

namespace DMAS_EXAM.DTOs;

/// <summary>
/// Request body for POST /api/createasset
/// </summary>
public class CreateAssetRequest
{
    [Required]
    [MaxLength(64)]
    public string AssetName { get; set; } = string.Empty;

    public int LevelRequire { get; set; }
}
