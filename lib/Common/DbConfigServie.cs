using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace GLT
{
    public class DbConfigServie
    {
        private IServiceProvider _service;
        private IConfiguration _config;

        public DbConfigServie(IServiceProvider service)
        {
            this._service = service;
            this._config = service.GetConfiguration<DbConfigServie>();
        }

        [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
        public DbConnectionString CoreDB_R() => _config.GetValue<string>();
    }
}
