using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOfficeApi.Data;
using MyOfficeApi.DTOs;
using MyOfficeApi.Models;
using MyOfficeApi.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MyOfficeApi.Controllers;

[ApiController]
[Route("api/myofficeacpd")]
[Produces("application/json")]
public class MyOfficeAcpdController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly LogService _logService;

    public MyOfficeAcpdController(AppDbContext context, LogService logService)
    {
        _context = context;
        _logService = logService;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "查詢所有資料", Description = "取得所有 MyOffice_ACPD 資料")]
    [SwaggerResponse(200, "成功取得資料", typeof(IEnumerable<MyOfficeAcpdResponseDto>))]
    public async Task<ActionResult<IEnumerable<MyOfficeAcpdResponseDto>>> GetAll()
    {
        try
        {
            var items = await _context.MyOfficeAcpd.ToListAsync();
            var result = items.Select(MapToResponseDto).ToList();
            
            await _logService.LogApiActionAsync("MyOfficeAcpd", "GetAll", 
                requestData: null, 
                responseData: new { Count = result.Count });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            await _logService.LogApiActionAsync("MyOfficeAcpd", "GetAll", error: ex.Message);
            return StatusCode(500, new { message = "伺服器內部錯誤", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "查詢單筆資料", Description = "根據 ID 取得單筆 MyOffice_ACPD 資料")]
    [SwaggerResponse(200, "成功取得資料", typeof(MyOfficeAcpdResponseDto))]
    [SwaggerResponse(404, "資源不存在")]
    public async Task<ActionResult<MyOfficeAcpdResponseDto>> GetById(string id)
    {
        try
        {
            var item = await _context.MyOfficeAcpd.FindAsync(id.PadRight(20));
            if (item == null)
            {
                await _logService.LogApiActionAsync("MyOfficeAcpd", "GetById", 
                    requestData: new { Id = id }, 
                    error: "資源不存在");
                return NotFound(new { message = "資源不存在" });
            }
            
            var result = MapToResponseDto(item);
            await _logService.LogApiActionAsync("MyOfficeAcpd", "GetById", 
                requestData: new { Id = id }, 
                responseData: new { Sid = result.Sid });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            await _logService.LogApiActionAsync("MyOfficeAcpd", "GetById", 
                requestData: new { Id = id }, 
                error: ex.Message);
            return StatusCode(500, new { message = "伺服器內部錯誤", error = ex.Message });
        }
    }

    [HttpPost]
    [SwaggerOperation(Summary = "新增資料", Description = "新增一筆 MyOffice_ACPD 資料")]
    [SwaggerResponse(201, "資源成功建立", typeof(MyOfficeAcpdResponseDto))]
    [SwaggerResponse(400, "請求參數有誤")]
    public async Task<ActionResult<MyOfficeAcpdResponseDto>> Create(
        [FromBody, SwaggerRequestBody("新增資料", Required = true)] MyOfficeAcpdCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await _logService.LogApiActionAsync("MyOfficeAcpd", "Create", 
                    requestData: dto, 
                    error: "請求參數有誤");
                return BadRequest(ModelState);
            }

            var newSid = await GenerateNewSidAsync();

            var entity = new MyOfficeAcpd
            {
                AcpdSid = newSid,
                AcpdCname = dto.Cname,
                AcpdEname = dto.Ename,
                AcpdSname = dto.Sname,
                AcpdEmail = dto.Email,
                AcpdStatus = dto.Status ?? 0,
                AcpdStop = dto.Stop ?? false,
                AcpdStopMemo = dto.StopMemo,
                AcpdLoginId = dto.LoginId,
                AcpdLoginPwd = dto.LoginPwd,
                AcpdMemo = dto.Memo,
                AcpdNowDateTime = DateTime.Now,
                AcpdNowId = dto.NowId,
                AcpdUpdDateTime = DateTime.Now,
                AcpdUpdId = dto.NowId
            };

            _context.MyOfficeAcpd.Add(entity);
            await _context.SaveChangesAsync();

            var result = MapToResponseDto(entity);
            await _logService.LogApiActionAsync("MyOfficeAcpd", "Create", 
                requestData: dto, 
                responseData: new { Sid = result.Sid });

            return CreatedAtAction(nameof(GetById), new { id = entity.AcpdSid.Trim() }, result);
        }
        catch (Exception ex)
        {
            await _logService.LogApiActionAsync("MyOfficeAcpd", "Create", 
                requestData: dto, 
                error: ex.Message);
            return StatusCode(500, new { message = "伺服器內部錯誤", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "更新資料", Description = "根據 ID 更新 MyOffice_ACPD 資料")]
    [SwaggerResponse(200, "請求成功", typeof(MyOfficeAcpdResponseDto))]
    [SwaggerResponse(400, "請求參數有誤")]
    [SwaggerResponse(404, "資源不存在")]
    public async Task<ActionResult<MyOfficeAcpdResponseDto>> Update(
        string id,
        [FromBody, SwaggerRequestBody("更新資料", Required = true)] MyOfficeAcpdUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await _logService.LogApiActionAsync("MyOfficeAcpd", "Update", 
                    requestData: new { Id = id, Data = dto }, 
                    error: "請求參數有誤");
                return BadRequest(ModelState);
            }

            var entity = await _context.MyOfficeAcpd.FindAsync(id.PadRight(20));
            if (entity == null)
            {
                await _logService.LogApiActionAsync("MyOfficeAcpd", "Update", 
                    requestData: new { Id = id, Data = dto }, 
                    error: "資源不存在");
                return NotFound(new { message = "資源不存在" });
            }

            entity.AcpdCname = dto.Cname ?? entity.AcpdCname;
            entity.AcpdEname = dto.Ename ?? entity.AcpdEname;
            entity.AcpdSname = dto.Sname ?? entity.AcpdSname;
            entity.AcpdEmail = dto.Email ?? entity.AcpdEmail;
            entity.AcpdStatus = dto.Status ?? entity.AcpdStatus;
            entity.AcpdStop = dto.Stop ?? entity.AcpdStop;
            entity.AcpdStopMemo = dto.StopMemo ?? entity.AcpdStopMemo;
            entity.AcpdLoginId = dto.LoginId ?? entity.AcpdLoginId;
            entity.AcpdLoginPwd = dto.LoginPwd ?? entity.AcpdLoginPwd;
            entity.AcpdMemo = dto.Memo ?? entity.AcpdMemo;
            entity.AcpdUpdDateTime = DateTime.Now;
            entity.AcpdUpdId = dto.UpdId ?? entity.AcpdUpdId;

            await _context.SaveChangesAsync();

            var result = MapToResponseDto(entity);
            await _logService.LogApiActionAsync("MyOfficeAcpd", "Update", 
                requestData: new { Id = id, Data = dto }, 
                responseData: new { Sid = result.Sid });

            return Ok(result);
        }
        catch (Exception ex)
        {
            await _logService.LogApiActionAsync("MyOfficeAcpd", "Update", 
                requestData: new { Id = id, Data = dto }, 
                error: ex.Message);
            return StatusCode(500, new { message = "伺服器內部錯誤", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "刪除資料", Description = "根據 ID 刪除 MyOffice_ACPD 資料")]
    [SwaggerResponse(204, "請求成功但無回傳內容")]
    [SwaggerResponse(404, "資源不存在")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var entity = await _context.MyOfficeAcpd.FindAsync(id.PadRight(20));
            if (entity == null)
            {
                await _logService.LogApiActionAsync("MyOfficeAcpd", "Delete", 
                    requestData: new { Id = id }, 
                    error: "資源不存在");
                return NotFound(new { message = "資源不存在" });
            }

            _context.MyOfficeAcpd.Remove(entity);
            await _context.SaveChangesAsync();

            await _logService.LogApiActionAsync("MyOfficeAcpd", "Delete", 
                requestData: new { Id = id }, 
                responseData: new { Deleted = true });

            return NoContent();
        }
        catch (Exception ex)
        {
            await _logService.LogApiActionAsync("MyOfficeAcpd", "Delete", 
                requestData: new { Id = id }, 
                error: ex.Message);
            return StatusCode(500, new { message = "伺服器內部錯誤", error = ex.Message });
        }
    }

    private static MyOfficeAcpdResponseDto MapToResponseDto(MyOfficeAcpd entity)
    {
        return new MyOfficeAcpdResponseDto
        {
            Sid = entity.AcpdSid.Trim(),
            Cname = entity.AcpdCname,
            Ename = entity.AcpdEname,
            Sname = entity.AcpdSname,
            Email = entity.AcpdEmail,
            Status = entity.AcpdStatus,
            Stop = entity.AcpdStop,
            StopMemo = entity.AcpdStopMemo,
            LoginId = entity.AcpdLoginId,
            Memo = entity.AcpdMemo,
            NowDateTime = entity.AcpdNowDateTime,
            NowId = entity.AcpdNowId,
            UpdDateTime = entity.AcpdUpdDateTime,
            UpdId = entity.AcpdUpdId
        };
    }

    /// <summary>
    /// 產生唯一主鍵 (參考 NewSID Stored Procedure)
    /// 格式: [年份編碼2碼][年中天數3碼][當日秒數5碼][隨機值10碼] = 20碼
    /// </summary>
    private async Task<string> GenerateNewSidAsync()
    {
        const string alphabets = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        while (true)
        {
            var now = DateTime.Now;
            var currentYear = now.Year - 2000;
            
            if (currentYear > 1295)
                currentYear = 1295;
            
            var dayOfYear = now.DayOfYear;
            var secondOfDay = now.Second + (60 * now.Minute) + (3600 * now.Hour);

            var firstDigit = alphabets[(currentYear / 36) % 36];
            var secondDigit = alphabets[currentYear % 36];
            var prefix = $"{firstDigit}{secondDigit}";
            var dayCode = dayOfYear.ToString("D3");
            var secondCode = secondOfDay.ToString("D5");

            var guidBytes = Guid.NewGuid().ToByteArray();
            var randomNumber = Math.Abs(BitConverter.ToInt64(guidBytes, 0)) % 10000000000;
            var randomValue = randomNumber.ToString("D10");

            var newSid = (prefix + dayCode + secondCode + randomValue).PadRight(20);

            var exists = await _context.MyOfficeAcpd.AnyAsync(x => x.AcpdSid == newSid);
            if (!exists)
                return newSid;
        }
    }
}
