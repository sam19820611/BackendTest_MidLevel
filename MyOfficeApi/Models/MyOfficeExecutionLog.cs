using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyOfficeApi.Models;

/// <summary>
/// 系統執行記錄
/// </summary>
[Table("MyOffice_ExcuteionLog")]
public class MyOfficeExecutionLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("DeLog_AutoID")]
    public long DeLogAutoId { get; set; }

    [Required]
    [Column("DeLog_StoredPrograms")]
    [StringLength(120)]
    public string DeLogStoredPrograms { get; set; } = string.Empty;

    [Required]
    [Column("DeLog_GroupID")]
    public Guid DeLogGroupId { get; set; }

    [Column("DeLog_isCustomDebug")]
    public bool DeLogIsCustomDebug { get; set; } = false;

    [Required]
    [Column("DeLog_ExecutionProgram")]
    [StringLength(120)]
    public string DeLogExecutionProgram { get; set; } = string.Empty;

    [Column("DeLog_ExecutionInfo")]
    public string? DeLogExecutionInfo { get; set; }

    [Column("DeLog_verifyNeeded")]
    public bool? DeLogVerifyNeeded { get; set; } = false;

    [Column("DeLog_ExDateTime")]
    public DateTime DeLogExDateTime { get; set; } = DateTime.Now;
}
