# 後端工程師技術測試 - MyOffice ACPD API

.NET Core 8 Web API 專案，實作 MyOffice_ACPD 資料表的完整 CRUD 功能。

## 專案結構

```
BackendTest_MidLevel/
├── MyOfficeApi/                         # Web API 專案
│   ├── Controllers/
│   │   └── MyOfficeAcpdController.cs    # RESTful API 控制器
│   ├── Data/
│   │   └── AppDbContext.cs              # Entity Framework DbContext
│   ├── DTOs/
│   │   └── MyOfficeAcpdDto.cs           # 資料傳輸物件
│   ├── Models/
│   │   ├── MyOfficeAcpd.cs              # 使用者資料 Model
│   │   └── MyOfficeExecutionLog.cs      # 執行日誌 Model
│   ├── Services/
│   │   └── LogService.cs                # 日誌服務 (呼叫 usp_AddLog SP)
│   ├── appsettings.json                 # 應用程式設定
│   └── Program.cs                       # 應用程式進入點
├── Myoffice_ACPD.bak                    # SQL Server 資料庫備份檔
├── SAMBackendTest_MidLevel.sln          # Visual Studio 方案檔
└── README.md                            # 專案說明文件
```

## 技術規格

| 項目 | 規格 |
|------|------|
| Framework | .NET 8.0 |
| ORM | Entity Framework Core 8.0 |
| Database | SQL Server 2019+ |
| API Documentation | Swagger / OpenAPI |

## 環境需求

- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server 2019+

## 執行步驟

### 1. 還原資料庫

使用 SQL Server Management Studio 還原 `Myoffice_ACPD.bak`：

```sql
RESTORE DATABASE [Myoffice_ACPD] 
FROM DISK = 'path\to\Myoffice_ACPD.bak'
WITH MOVE 'Myoffice_ACPD' TO 'C:\...\Myoffice_ACPD.mdf',
     MOVE 'Myoffice_ACPD_log' TO 'C:\...\Myoffice_ACPD_log.ldf';
```

### 2. 設定連線字串

修改 `MyOfficeApi/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=Myoffice_ACPD;User ID=sa;Password=您的密碼;TrustServerCertificate=True"
  }
}
```

### 3. 執行專案

1. 開啟 `SAMBackendTest_MidLevel.sln`
2. 按 **F5** 執行
3. Swagger UI 自動開啟

## API 端點

| HTTP Method | URL | 說明 | 狀態碼 |
|-------------|-----|------|--------|
| `GET` | `/api/myofficeacpd` | 查詢所有資料 | 200 |
| `GET` | `/api/myofficeacpd/{id}` | 查詢單筆資料 | 200 / 404 |
| `POST` | `/api/myofficeacpd` | 新增資料 | 201 / 400 |
| `PUT` | `/api/myofficeacpd/{id}` | 更新資料 | 200 / 400 / 404 |
| `DELETE` | `/api/myofficeacpd/{id}` | 刪除資料 | 204 / 404 |

## 測試範例 (Swagger JSON)

### POST - 新增資料

```json
{
  "cname": "王小明",
  "ename": "Wang Xiao Ming",
  "sname": "小明",
  "email": "xiaoming@example.com",
  "status": 0,
  "stop": false,
  "loginId": "xiaoming",
  "loginPwd": "password123",
  "memo": "測試用戶",
  "nowId": "admin"
}
```

### PUT - 更新資料

```json
{
  "cname": "王小明",
  "email": "newemail@example.com",
  "memo": "已更新資料",
  "updId": "admin"
}
```

## 資料表結構

### MyOffice_ACPD (使用者基本資訊)

| 欄位名稱 | 型態 | 說明 |
|----------|------|------|
| ACPD_SID | char(20) | 主鍵 |
| ACPD_Cname | nvarchar(60) | 中文名稱 |
| ACPD_Ename | nvarchar(40) | 英文名稱 |
| ACPD_Sname | nvarchar(40) | 簡稱 |
| ACPD_Email | nvarchar(60) | 電子郵件 |
| ACPD_Status | tinyint | 狀態 (0=正常, 99=不正常) |
| ACPD_Stop | bit | 是否停用 |
| ACPD_StopMemo | nvarchar(60) | 停用原因 |
| ACPD_LoginID | nvarchar(30) | 登入帳號 |
| ACPD_LoginPWD | nvarchar(60) | 登入密碼 |
| ACPD_Memo | nvarchar(600) | 備註 |
| ACPD_NowDateTime | datetime | 新增日期 |
| ACPD_NowID | nvarchar(20) | 新增人員代碼 |
| ACPD_UPDDateTime | datetime | 修改日期 |
| ACPD_UPDID | nvarchar(20) | 修改人員代碼 |

### MyOffice_ExcuteionLog (執行日誌)

| 欄位名稱 | 型態 | 說明 |
|----------|------|------|
| DeLog_AutoID | bigint | 自動編號主鍵 |
| DeLog_StoredPrograms | nvarchar(120) | 預存程序名稱 |
| DeLog_GroupID | uniqueidentifier | 執行群組代碼 |
| DeLog_isCustomDebug | bit | 是否為 Debug |
| DeLog_ExecutionProgram | nvarchar(120) | 執行動作 |
| DeLog_ExecutionInfo | nvarchar(max) | 執行內容 (JSON) |
| DeLog_verifyNeeded | bit | 是否需要檢查 |
| DeLog_ExDateTime | datetime | 執行時間 |

## 主鍵產生規則 (NewSID)

主鍵 (ACPD_SID) 採用自訂格式，共 20 碼：

| 位置 | 長度 | 說明 | 範例 |
|------|------|------|------|
| 1-2 | 2 碼 | 年份編碼 (36進位) | `0Q` |
| 3-5 | 3 碼 | 年中第幾天 | `086` |
| 6-10 | 5 碼 | 當日秒數 | `15420` |
| 11-20 | 10 碼 | GUID 隨機值 | `K3G3bHJ5c0` |

```
0Q08615420K3G3bHJ5c0
│││   │    └─────────── 隨機值 (10碼)
│││   └──────────────── 當日秒數 (5碼)
││└──────────────────── 年中天數 (3碼)
└┴───────────────────── 年份編碼 (2碼)
```

## 日誌功能

API 操作會透過 `LogService` 呼叫 `usp_AddLog` Stored Procedure 記錄執行日誌。

## Git 分支管理

| 分支 | 說明 |
|------|------|
| `main` | 最終版本 |
| `develop` | 開發主分支 |
| `feature/crud-api` | 功能開發分支 |

## 繳交清單

- [x] GitHub Repository（包含分支管理記錄）
- [x] .NET Core 8 Web API 專案原始碼
- [x] Swagger 可正常執行 CRUD，每個 API 附有測試 JSON
- [x] SQL Server 資料庫備份檔 `.bak`（包含測試資料）
- [x] README 說明專案架構與執行步驟

## GitHub Repository

https://github.com/sam19820611/BackendTest_MidLevel
