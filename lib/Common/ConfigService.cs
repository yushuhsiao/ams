using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Data;

namespace GLT
{
    public class ConfigService
    {
        private IConfiguration _config;

        public DbConfigServie Db { get; }

        public ConfigService(IServiceProvider service)
        {
            this._config = service.GetConfiguration<DbConfigServie>();
            this.Db = ActivatorUtilities.CreateInstance<DbConfigServie>(service);
        }

        public class DbConfigServie
        {
            private IConfiguration _config;

            public DbConfigServie(IServiceProvider service)
            {
                this._config = service.GetConfiguration<DbConfigServie>();
            }

            [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
            public DbConnectionString CoreDB_R() => _config.GetValue<string>();

            [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
            public DbConnectionString CoreDB_W() => _config.GetValue<string>();
        }
    }

    //internal class DbConnectionFactory : MiniProfiler.Integrations.IDbConnectionFactory
    //{
    //}
}
