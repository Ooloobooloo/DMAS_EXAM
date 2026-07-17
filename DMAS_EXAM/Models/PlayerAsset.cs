using System.ComponentModel.DataAnnotations.Schema;

namespace DMAS_EXAM.Models;

public class PlayerAsset
{
 [ForeignKey("PlayerId")] 
 public virtual int PlayerId { get; set; }
 [ForeignKey("AssetId")] 
 public int AssetId { get; set; }
 
 
 
}