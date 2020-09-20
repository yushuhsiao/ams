# 執行環境
    Microsoft SQL Server 2016 Service Pack 2
    Redis (Docker Official Images)
    .Net Core 3.1

# 開發環境
    Windows 10
    .Net Core SDK 3.1.300
    Visual Studio 2019
    Docker Desktop

## 引用的套件
    Microsoft.NETCore.App       v3.1.0
    Microsoft.AspNetCore.App    v3.1.2
    Newtonsoft.Json             v12.0.3
    StackExchange.Redis         v2.1.30
    Dapper                      v2.0.35
    Swashbuckle.AspNetCore      v5.4.1

## Api 格式
    ContentType: application/json;
    ResponseBody:

    {
      "Code": int,      (1=Success)
      "Msg": string,
      "Rows": [ object1, object2, ...],
      "RowIndex": int,    (optional)
      "RowCount": int,    (optional)
      "Error": object     (optional when error)
    }


## 計劃項目
     Create Corp
     Modify Corp
     Create Admin
     Modify Admin
    *ACL Define
     ACL Group
     ACL Verify
    帳號額度管理
    會員資料庫/相關處理程序
    主要管理後台 - blazor webassembly
        代理/會員管理
            詳細資料
            限額設定
        子帳號管理
        系統設定
        外接平台
            帳務資料同步
            狀態監控        
        控盤後台
    金流介接
    遊戲介接
    展示用前台 - server-side blazor + webix



# 資料庫
    CMS_Core        系統核心
    CMS_Main        會員網站，Main Office 相關事務
    CMS_EventLog    事件紀錄
    CMS_Reporting   報表
    CMS_User        站台的會員資料
    CMS_Log         站台的帳務資料



# 資料夾結構
    app
        api_test
        CMS_API         API Server
    db                  Database Schemas
    lib                 Librarys
        Common          Common Library
        Tools           Utilities
