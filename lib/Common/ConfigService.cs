using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Data;

namespace CMS
{
    public static class ConfigServiceExtension
    {
        public static IServiceCollection AddConfigService(this IServiceCollection services)
        {
            services.AddConfigurationBinder();
            services.AddSingleton<ConfigService>();
            return services;
        }
    }
    public sealed class ConfigService
    {
        public _Database Db { get; }
        private IConfiguration _config;
        //public _Redis Redis { get; }

        public ConfigService(IServiceProvider service)
        {
            this.Db = ActivatorUtilities.CreateInstance<_Database>(service);
            this._config = service.GetConfiguration<ConfigService>();
        }

        [AppSetting(SectionName = ""), DefaultValue(0)]
        public int DefaultCorpId => _config.GetValue<int>();

        [AppSetting(SectionName = ""), DefaultValue(0)]
        public string DefaultCorpName => _config.GetValue<string>();

        public bool IsRootCorp
        {
            get
            {
                var d = DefaultCorpId;
                return d == 0 || d == 1;
            }
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

            [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.Database.UserDB_R), DefaultValue(_Consts.Database.UserDB_Default)]
            public DbConnectionString UserDB_R(int corpId) => _config.GetValue<string>(corpId);

            [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.Database.UserDB_W), DefaultValue(_Consts.Database.UserDB_Default)]
            public DbConnectionString UserDB_W(int corpId) => _config.GetValue<string>(corpId);
        }
    }

    //internal class DbConnectionFactory : MiniProfiler.Integrations.IDbConnectionFactory
    //{
    //}
}
