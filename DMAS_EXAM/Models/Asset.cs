using System.ComponentModel.DataAnnotations;

namespace DMAS_EXAM.Models;

public class Asset
{
    [Key]
    public int AssetId { get; set; }
    
    public string AssetName { get; set; } = string.Empty;
    
    public int LevelRequire { get; set; } 
    
    
}