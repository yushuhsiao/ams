using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml.Schema;
using Dapper;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CMS
{
    public class AclDefineProvider : IDataService
    {
        private DataService _service;
        private ILogger _logger;
        private DbCache<Entity.AclDefine> _cache;

        public AclDefineProvider(DataService dataService)
        {
            _service = dataService;
            _logger = _service.GetService<ILogger<AclDefineProvider>>();
            _cache = _service.GetDbCache<Entity.AclDefine>(_cache_ReadDataEvent);
        }

        private IEnumerable<Entity.AclDefine> _cache_ReadDataEvent(DbCache<Entity.AclDefine>.Entry sender, Entity.AclDefine[] oldValue)
        {
            using (var core_db = _service.Db.CoreDB_R())
            {
                var result = core_db.Query<Entity.AclDefine>($"select * from {TableName<Entity.AclDefine>.Value}");
                var dict = result.ToDictionary(x => x.Id);
                return result;
            }
        }

        private List<ApiAttribute> GetApis()
        {
            List<ApiAttribute> apis = new List<ApiAttribute>();
            var parts = _service.GetService<ApplicationPartManager>();
            foreach (var a in parts.ApplicationParts)
            {
                if (a is AssemblyPart b)
                {
                    if (b.Assembly.GetCustomAttribute<ApiAttribute>() != null)
                    {
                        foreach (var c in b.Types)
                        {
                            foreach (var m in c.GetMethods())
                            {
                                var apiAttr = m.GetCustomAttribute<ApiAttribute>();
                                if (apiAttr != null)
                                {
                                    apis.Add(apiAttr);
                                }
                            }
                        }
                    }
                }
            }
            return apis;
        }

        public void Init()
        {
            var apis = GetApis();
            var acls = _cache.GetValues();
            //core_db.Query<Entity.AclDefine>("select * from AclDefine").ToDictionary(x => x.Id);
            IDbTransaction sql_tran = null;
            try
            {
                foreach (var apiAttr in apis)
                {
                    string[] names = apiAttr.ACL_Name?.Split(".");
                    if (names == null) continue;
                    Entity.AclDefine parent = null;
                    string fullName = null;
                    foreach (var name in names)
                    {
                        int parentId = parent?.Id ?? 0;
                        var item = acls.FirstOrDefault(x => x.ParentId == parentId && x.Name.IsEquals(name, true));
                        if (fullName == null)
                            fullName = name;
                        else
                            fullName = $"{fullName}.{name}";
                        if (item == null)
                        {
                            if (sql_tran == null)
                                sql_tran = _service.GetService<DbConnectionService>().CoreDB_W().BeginTransaction();
                            item = sql_tran.QuerySingle<Entity.AclDefine>($@"
 insert into AclDefine ( ParentId, Name, FullName, Flags)
 values                (@ParentId,@Name,@FullName,@Flags);
 select * from AclDefine where Id = @@IDENTITY; ", new
                            {
                                ParentId = parentId,
                                Name = name,
                                FullName = fullName,
                                Flags = (int)apiAttr.Flags,
                            });
                            acls = acls.Add(item);
                        }
                        parent = item;
                    }
                }
                if (sql_tran != null)
                {
                    sql_tran.Commit();
                    _cache.UpdateVersion();
                    _cache.GetValues();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                using (sql_tran?.Connection)
                using (sql_tran)
                {
                }
            }
        }
    }
}
