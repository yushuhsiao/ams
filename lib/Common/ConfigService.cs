using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Data;

namespace GLT
{
    public sealed class ConfigService
    {
        public _Database Db { get; }
        //public _Redis Redis { get; }

        public ConfigService(IServiceProvider service)
        {
            this.Db = ActivatorUtilities.CreateInstance<_Database>(service);
            //this.Redis = ActivatorUtilities.CreateInstance<_Redis>(service);
        }

        //public sealed class _Redis
        //{
        //    private IConfiguration _config;
        //    public _Redis(IServiceProvider service)
        //    {
        //        this._config = service.GetConfiguration<_Redis>();
        //    }

        //    [AppSetting(SectionName = _Consts.Redis.Key1), DefaultValue(_Consts.Redis.TableVer_DefaultValue)]
        //    public string TableVer => _config.GetValue<string>();
        //}

        public sealed class _Database
        {
            private IConfiguration _config;

            public _Database(IServiceProvider service)
            {
                this._config = service.GetConfiguration<_Database>();
            }

            [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.Database.CoreDB_R), DefaultValue(_Consts.Database.CoreDB_Default)]
            public DbConnectionString CoreDB_R() => _config.GetValue<string>();

            [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.Database.CoreDB_W), DefaultValue(_Consts.Database.CoreDB_Default)]
            public DbConnectionString CoreDB_W() => _config.GetValue<string>();
        }
    }

    //internal class DbConnectionFactory : MiniProfiler.Integrations.IDbConnectionFactory
    //{
    //}
}
