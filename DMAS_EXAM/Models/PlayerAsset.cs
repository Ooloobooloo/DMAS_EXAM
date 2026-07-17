using System.ComponentModel.DataAnnotations.Schema;

namespace DMAS_EXAM.Models;

public class PlayerAsset
{
 
 public virtual int PlayerId { get; set; }
 [ForeignKey("PlayerId")] 
 public virtual Player? Player { get; set; }
 
 
 public int AssetId { get; set; } 
 [ForeignKey("AssetId")] 
 public virtual Asset? Asset { get; set; }
 
}