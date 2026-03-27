# MyOffice ACPD API

.NET Core 8 Web API 專案，提供 MyOffice_ACPD 資料表的 CRUD 操作。

## 專案架構

```
MyOfficeApi/
├── Controllers/
│   └── MyOfficeAcpdController.cs    # RESTful API 控制器
├── Data/
│   └── AppDbContext.cs              # Entity Framework DbContext
├── DTOs/
│   └── MyOfficeAcpdDto.cs           # 資料傳輸物件 (Create/Update/Response)
├── Models/
│   └── MyOfficeAcpd.cs              # 資料庫實體模型
├── Properties/
│   └── launchSettings.json          # 啟動設定
├── appsettings.json                  # 應用程式設定（含資料庫連線）
├── Program.cs                        # 應用程式進入點
└── MyOfficeApi.csproj               # 專案檔
```

## 技術規格

- **Framework**: .NET 8.0
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server 2019+
- **API Documentation**: Swagger / OpenAPI

## 執行步驟

### 1. 環境需求

- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server 2019+

### 2. 資料庫設定

1. 還原 `Myoffice_ACPD` 資料庫備份檔 (.bak)
2. 或執行 `TSQLScript/TSQL_Myoffice_ACPD.sql` 建立資料表

### 3. 設定連線字串

修改 `appsettings.json` 中的連線字串：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=Myoffice_ACPD;User ID=sa;Password=您的密碼;TrustServerCertificate=True"
  }
}
```

### 4. 執行專案

- 在 Visual Studio 2022 中按 **F5** 執行
- Swagger UI 會自動開啟

## API 端點

| HTTP Method | URL | 說明 | 回應狀態碼 |
|-------------|-----|------|-----------|
| `GET` | `/api/myofficeacpd` | 查詢所有資料 | 200 |
| `GET` | `/api/myofficeacpd/{id}` | 查詢單筆資料 | 200 / 404 |
| `POST` | `/api/myofficeacpd` | 新增資料 | 201 / 400 |
| `PUT` | `/api/myofficeacpd/{id}` | 更新資料 | 200 / 400 / 404 |
| `DELETE` | `/api/myofficeacpd/{id}` | 刪除資料 | 204 / 404 |

## 測試範例

### 新增資料 (POST)

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

### 更新資料 (PUT)

```json
{
  "cname": "王小明",
  "email": "newemail@example.com",
  "memo": "已更新資料",
  "updId": "admin"
}
```

## 資料表結構

| 欄位名稱 | 型態 | 說明 |
|----------|------|------|
| ACPD_SID | char(20) | 主鍵 |
| ACPD_Cname | nvarchar(60) | 中文名稱 |
| ACPD_Ename | nvarchar(40) | 英文名稱 |
| ACPD_Sname | nvarchar(40) | 簡稱 |
| ACPD_Email | nvarchar(60) | 電子郵件 |
| ACPD_Status | tinyint | 狀態 (0=正常, 99=不正常) |
| ACPD_Stop | bit | 是否停用 |
| ACPD_StopMemo | nvarchar(600) | 停用原因 |
| ACPD_LoginID | nvarchar(30) | 登入帳號 |
| ACPD_LoginPWD | nvarchar(60) | 登入密碼 |
| ACPD_Memo | nvarchar(120) | 備註 |
| ACPD_NowDateTime | datetime | 新增日期 |
| ACPD_NowID | nvarchar(20) | 新增人員代碼 |
| ACPD_UPDDateTime | datetime | 修改日期 |
| ACPD_UPDID | nvarchar(20) | 修改人員代碼 |

## 主鍵產生規則

主鍵 (ACPD_SID) 參照 `NewSID` Stored Procedure 產生，採用自訂格式：

### 格式結構

| 位置 | 長度 | 說明 | 範例 |
|------|------|------|------|
| 1-2 | 2 碼 | 年份編碼 (36進位) | `0Q` |
| 3-5 | 3 碼 | 年中第幾天 (001-366) | `086` |
| 6-10 | 5 碼 | 當日秒數 (00000-86399) | `15420` |
| 11-20 | 10 碼 | GUID 隨機值 | `K3G3bHJ5c0` |

### 年份編碼說明

使用 36 進位編碼 (0-9, A-Z)：
- 第 1 碼 = `(年份 - 2000) / 36`
- 第 2 碼 = `(年份 - 2000) % 36`
- 範例：2026 年 → `0` + `Q` = `0Q`

### 唯一性保證

- 使用 `Guid.NewGuid()` 產生隨機值
- 自動檢查資料庫確保 SID 不重複

### 範例

```
0Q08615420K3G3bHJ5c0
│││   │    │
││└───┴────┴─── 隨機值 (10碼)
│└───────────── 當日秒數 (5碼)
└────────────── 年中天數 (3碼)
年份編碼 (2碼)
```
