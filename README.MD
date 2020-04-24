# 執行環境
    Microsoft SQL Server 2016 Service Pack 2
    Redis (Docker Official Images)
    .Net Core 3.1

# 開發環境
    Windows 10
    .Net Core SDK 3.1.201
    Visual Studio 2019
    Docker Desktop

## 引用的套件
    Microsoft.NETCore.App       v3.1.0
    Microsoft.AspNetCore.App    v3.1.3
    Newtonsoft.Json             v12.0.2
    StackExchange.Redis         v2.1.30
    Dapper                      v2.0.35
    Swashbuckle.AspNetCore      v5.3.3

## 計劃項目
    docker 設定


# 資料庫
    GLT_Core        系統核心
    GLT_Main        會員網站，Main Office 相關事務
    GLT_EventLog    事件紀錄
    GLT_Reporting   報表
    GLT_User        站台的會員資料
    GLT_Log         站台的帳務資料



# 資料夾結構
    app
        api_test
        GLT_API         API Server
    db                  Database Schemas
    lib                 Librarys
        Common          Common Library
        Tools           Utilities
