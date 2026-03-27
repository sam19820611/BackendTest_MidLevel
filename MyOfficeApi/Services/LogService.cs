using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyOfficeApi.Data;

namespace MyOfficeApi.Services;

/// <summary>
/// 日誌記錄服務 - 呼叫 usp_AddLog Stored Procedure
/// </summary>
public class LogService
{
    private readonly AppDbContext _context;

    public LogService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 呼叫 usp_AddLog Stored Procedure 新增執行日誌
    /// </summary>
    /// <param name="readId">執行 Log 時是使用第幾版 (預設 0)</param>
    /// <param name="spName">執行的預存程序名稱</param>
    /// <param name="groupId">執行群組代碼</param>
    /// <param name="exProgram">執行的動作是什麼</param>
    /// <param name="actionJson">執行的過程是什麼 (JSON)</param>
    /// <returns>回傳執行的項目 (JSON)</returns>
    public async Task<string?> AddLogAsync(
        byte readId,
        string spName,
        Guid groupId,
        string exProgram,
        string? actionJson = null)
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            
            using var command = connection.CreateCommand();
            command.CommandText = "usp_AddLog";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@_InBox_ReadID", SqlDbType.TinyInt) { Value = readId });
            command.Parameters.Add(new SqlParameter("@_InBox_SPNAME", SqlDbType.NVarChar, 120) { Value = spName });
            command.Parameters.Add(new SqlParameter("@_InBox_GroupID", SqlDbType.UniqueIdentifier) { Value = groupId });
            command.Parameters.Add(new SqlParameter("@_InBox_ExProgram", SqlDbType.NVarChar, 40) { Value = exProgram });
            command.Parameters.Add(new SqlParameter("@_InBox_ActionJSON", SqlDbType.NVarChar, -1) { Value = actionJson ?? (object)DBNull.Value });
            
            var outputParam = new SqlParameter("@_OutBox_ReturnValues", SqlDbType.NVarChar, -1)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputParam);

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();

            return outputParam.Value as string;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// 記錄 API 操作 (簡化方法)
    /// </summary>
    public async Task<string?> LogApiActionAsync(
        string apiName, 
        string action, 
        object? requestData = null, 
        object? responseData = null, 
        string? error = null)
    {
        var groupId = Guid.NewGuid();
        var info = new
        {
            Request = requestData,
            Response = responseData,
            Error = error,
            Timestamp = DateTime.Now
        };

        var actionJson = JsonSerializer.Serialize(info, new JsonSerializerOptions
        {
            WriteIndented = false,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        return await AddLogAsync(
            readId: 0,
            spName: $"API_{apiName}",
            groupId: groupId,
            exProgram: action,
            actionJson: actionJson
        );
    }
}
