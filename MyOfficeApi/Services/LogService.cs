using System.Text.Json;
using MyOfficeApi.Data;
using MyOfficeApi.Models;

namespace MyOfficeApi.Services;

/// <summary>
/// 日誌記錄服務 (參考 usp_AddLog)
/// </summary>
public class LogService
{
    private readonly AppDbContext _context;

    public LogService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 新增執行日誌
    /// </summary>
    /// <param name="spName">預存程序/API 名稱</param>
    /// <param name="groupId">執行群組代碼</param>
    /// <param name="program">執行的動作</param>
    /// <param name="info">執行的內容 (JSON)</param>
    /// <param name="isDebug">是否為 Debug</param>
    /// <param name="verifyNeeded">是否需要檢查</param>
    public async Task AddLogAsync(
        string spName,
        Guid groupId,
        string program,
        object? info = null,
        bool isDebug = false,
        bool verifyNeeded = false)
    {
        var log = new MyOfficeExecutionLog
        {
            DeLogStoredPrograms = spName,
            DeLogGroupId = groupId,
            DeLogExecutionProgram = program,
            DeLogExecutionInfo = info != null ? JsonSerializer.Serialize(info, new JsonSerializerOptions 
            { 
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }) : null,
            DeLogIsCustomDebug = isDebug,
            DeLogVerifyNeeded = verifyNeeded,
            DeLogExDateTime = DateTime.Now
        };

        _context.MyOfficeExecutionLog.Add(log);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// 記錄 API 操作
    /// </summary>
    public async Task LogApiActionAsync(string apiName, string action, object? requestData = null, object? responseData = null, string? error = null)
    {
        var groupId = Guid.NewGuid();
        var info = new
        {
            Request = requestData,
            Response = responseData,
            Error = error,
            Timestamp = DateTime.Now
        };

        await AddLogAsync(
            spName: $"API_{apiName}",
            groupId: groupId,
            program: action,
            info: info,
            isDebug: error != null,
            verifyNeeded: error != null
        );
    }
}
