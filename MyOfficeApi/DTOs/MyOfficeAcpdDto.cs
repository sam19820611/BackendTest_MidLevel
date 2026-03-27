using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyOfficeApi.DTOs;

/// <summary>
/// 新增使用者資料
/// </summary>
public class MyOfficeAcpdCreateDto
{
    /// <summary>中文名稱</summary>
    /// <example>王小明</example>
    [StringLength(60)]
    public string? Cname { get; set; }

    /// <summary>英文名稱</summary>
    /// <example>Wang Xiao Ming</example>
    [StringLength(40)]
    public string? Ename { get; set; }

    /// <summary>簡稱</summary>
    /// <example>小明</example>
    [StringLength(40)]
    public string? Sname { get; set; }

    /// <summary>電子郵件</summary>
    /// <example>xiaoming@example.com</example>
    [StringLength(60)]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>狀態 (0=正常, 99=不正常)</summary>
    /// <example>0</example>
    [DefaultValue(0)]
    public byte? Status { get; set; }

    /// <summary>是否停用</summary>
    /// <example>false</example>
    [DefaultValue(false)]
    public bool? Stop { get; set; }

    /// <summary>停用原因</summary>
    /// <example></example>
    [StringLength(60)]
    public string? StopMemo { get; set; }

    /// <summary>登入帳號</summary>
    /// <example>xiaoming</example>
    [StringLength(30)]
    public string? LoginId { get; set; }

    /// <summary>登入密碼</summary>
    /// <example>password123</example>
    [StringLength(60)]
    public string? LoginPwd { get; set; }

    /// <summary>備註</summary>
    /// <example>測試用戶</example>
    [StringLength(600)]
    public string? Memo { get; set; }

    /// <summary>新增人員代碼</summary>
    /// <example>admin</example>
    [StringLength(20)]
    public string? NowId { get; set; }
}

/// <summary>
/// 更新使用者資料
/// </summary>
public class MyOfficeAcpdUpdateDto
{
    /// <summary>中文名稱</summary>
    /// <example>王小明</example>
    [StringLength(60)]
    public string? Cname { get; set; }

    /// <summary>英文名稱</summary>
    /// <example>Wang Xiao Ming</example>
    [StringLength(40)]
    public string? Ename { get; set; }

    /// <summary>簡稱</summary>
    /// <example>小明</example>
    [StringLength(40)]
    public string? Sname { get; set; }

    /// <summary>電子郵件</summary>
    /// <example>xiaoming@example.com</example>
    [StringLength(60)]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>狀態 (0=正常, 99=不正常)</summary>
    /// <example>0</example>
    public byte? Status { get; set; }

    /// <summary>是否停用</summary>
    /// <example>false</example>
    public bool? Stop { get; set; }

    /// <summary>停用原因</summary>
    /// <example></example>
    [StringLength(60)]
    public string? StopMemo { get; set; }

    /// <summary>登入帳號</summary>
    /// <example>xiaoming</example>
    [StringLength(30)]
    public string? LoginId { get; set; }

    /// <summary>登入密碼</summary>
    /// <example>newpassword456</example>
    [StringLength(60)]
    public string? LoginPwd { get; set; }

    /// <summary>備註</summary>
    /// <example>已更新資料</example>
    [StringLength(600)]
    public string? Memo { get; set; }

    /// <summary>更新人員代碼</summary>
    /// <example>admin</example>
    [StringLength(20)]
    public string? UpdId { get; set; }
}

/// <summary>
/// 使用者資料回應
/// </summary>
public class MyOfficeAcpdResponseDto
{
    /// <summary>使用者主鍵</summary>
    /// <example>0Q08615aborK3G3bHJ5c</example>
    public string Sid { get; set; } = string.Empty;

    /// <summary>中文名稱</summary>
    /// <example>王小明</example>
    public string? Cname { get; set; }

    /// <summary>英文名稱</summary>
    /// <example>Wang Xiao Ming</example>
    public string? Ename { get; set; }

    /// <summary>簡稱</summary>
    /// <example>小明</example>
    public string? Sname { get; set; }

    /// <summary>電子郵件</summary>
    /// <example>xiaoming@example.com</example>
    public string? Email { get; set; }

    /// <summary>狀態</summary>
    /// <example>0</example>
    public byte? Status { get; set; }

    /// <summary>是否停用</summary>
    /// <example>false</example>
    public bool? Stop { get; set; }

    /// <summary>停用原因</summary>
    public string? StopMemo { get; set; }

    /// <summary>登入帳號</summary>
    /// <example>xiaoming</example>
    public string? LoginId { get; set; }

    /// <summary>備註</summary>
    /// <example>測試用戶</example>
    public string? Memo { get; set; }

    /// <summary>新增日期</summary>
    /// <example>2024-03-27T10:30:00</example>
    public DateTime? NowDateTime { get; set; }

    /// <summary>新增人員代碼</summary>
    /// <example>admin</example>
    public string? NowId { get; set; }

    /// <summary>修改日期</summary>
    /// <example>2024-03-27T10:30:00</example>
    public DateTime? UpdDateTime { get; set; }

    /// <summary>修改人員代碼</summary>
    /// <example>admin</example>
    public string? UpdId { get; set; }
}
