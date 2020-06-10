using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace GLT
{
    public class CorpInfoProvider : IDataService
    {
        DataService _service;

        public CorpInfoProvider(DataService service)
        {
            _service = service;
        }

        public Entity.CorpInfo GetCorp(int corpId)
        {
            using (var core_db = _service.Db.CoreDB_R())
            {
                var corp = core_db.QuerySingleOrDefault<Entity.CorpInfo>($@"
 select * from {TableName<Entity.CorpInfo>.Value} where Id = {corpId}; ");
                if (corp == null)
                {
                    if (corpId == 1)
                        return Create(corpId);
                    if (false == _service.Config.IsRootCorp &&
                        corpId == _service.Config.DefaultCorpId)
                        return Create(corpId);
                }
                return null;
            }
        }

        public Entity.CorpInfo Create(int corpId)
        {
            return null;
        }
    }
}
