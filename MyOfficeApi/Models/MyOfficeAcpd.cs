using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyOfficeApi.Models;

[Table("MyOffice_ACPD")]
public class MyOfficeAcpd
{
    [Key]
    [Column("ACPD_SID")]
    [StringLength(20)]
    public string AcpdSid { get; set; } = string.Empty;

    [Column("ACPD_Cname")]
    [StringLength(60)]
    public string? AcpdCname { get; set; }

    [Column("ACPD_Ename")]
    [StringLength(40)]
    public string? AcpdEname { get; set; }

    [Column("ACPD_Sname")]
    [StringLength(40)]
    public string? AcpdSname { get; set; }

    [Column("ACPD_Email")]
    [StringLength(60)]
    public string? AcpdEmail { get; set; }

    [Column("ACPD_Status")]
    public byte? AcpdStatus { get; set; }

    [Column("ACPD_Stop")]
    public bool? AcpdStop { get; set; }

    [Column("ACPD_StopMemo")]
    [StringLength(600)]
    public string? AcpdStopMemo { get; set; }

    [Column("ACPD_LoginID")]
    [StringLength(30)]
    public string? AcpdLoginId { get; set; }

    [Column("ACPD_LoginPWD")]
    [StringLength(60)]
    public string? AcpdLoginPwd { get; set; }

    [Column("ACPD_Memo")]
    [StringLength(120)]
    public string? AcpdMemo { get; set; }

    [Column("ACPD_NowDateTime")]
    public DateTime? AcpdNowDateTime { get; set; }

    [Column("ACPD_NowID")]
    [StringLength(20)]
    public string? AcpdNowId { get; set; }

    [Column("ACPD_UPDDateTime")]
    public DateTime? AcpdUpdDateTime { get; set; }

    [Column("ACPD_UPDID")]
    [StringLength(20)]
    public string? AcpdUpdId { get; set; }
}
