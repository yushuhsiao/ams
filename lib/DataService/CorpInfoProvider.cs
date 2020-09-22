using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using CMS.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace CMS
{
    public class CorpInfoProvider : IDataService
    {
        DataService _service;
        DbCache<Entity.CorpInfo> _cache;

        public CorpInfoProvider(DataService service)
        {
            _service = service;
            _cache = service.GetDbCache<Entity.CorpInfo>();
        }

        public Entity.CorpInfo GetCorp(int corpId)
        {
            using (var core_db = _service.Db.CoreDB_R())
            {
                var corp = core_db.QuerySingleOrDefault<Entity.CorpInfo>($@"
 select * from {TableName<Entity.CorpInfo>.Value} where Id = {corpId}; ");
                if (corp == null)
                {
                    // CorpId = 會自動建立, 如果建立失敗, 系統將會無法運作
                    if (corpId == _Consts.root.CorpId)
                    {
                        try
                        {
                            return Create(new Entity.CorpInfo
                            {
                                Id = corpId,
                                Name = _Consts.root.CorpName,
                                Mode = 0,
                                Active = ActiveState.Active,
                                Currency = CurrencyCode.Default,
                                Prefix = "",
                                CreateUser = 0
                            });
                        }
                        catch
                        {
                            throw new ApiException(Status.Exception, "Unable to create default corp")
                            {
                                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError
                            };
                        }
                    }
                }
                return null;
            }
        }

        public Entity.CorpInfo Create(Entity.CorpInfo data)
        {
            var sql = $@"
 insert into {TableName<Entity.CorpInfo>.Value} ( Id, Name, Mode, Active, Currency, Prefix, CreateTime, CreateUser, ModifyTime, ModifyUser)
 values                                         (@Id,@Name,@Mode,@Active,@Currency,@Prefix, getdate() ,@op_user, getdate() ,@op_user) ; 
 select * from {TableName<Entity.CorpInfo>.Value} where Id = @Id ;";
            using (var tran = _service.Db.CoreDB_WT())
            {
                var result = tran.QuerySingle<Entity.CorpInfo>(sql, new
                {
                    Id = data.Id,
                    Name = data.Name,
                    Mode = data.Mode,
                    Prefix = "",
                    Active = (int)data.Active,
                    Currency = (int)data.Currency,
                    op_user = data.CreateUser
                });
                tran.Commit();
                _cache.UpdateVersion();
                return result;
            }
        }
    }
}
