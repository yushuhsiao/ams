﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace GLT
{
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

    public class DbConnectionService
    {
        private IServiceProvider _service;
        public DbConfigServie Config { get; }

        public DbConnectionService(IServiceProvider service)
        {
            this._service = service;
            this.Config = new DbConfigServie(service);
        }

        internal IDbConnection OpenDbConnection(DbConnectionString connectionString)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public IDbConnection CoreDB_R() => Config.CoreDB_R().GetDbConnection(_service, OpenDbConnection);
        public IDbConnection CoreDB_W() => Config.CoreDB_W().GetDbConnection(_service, OpenDbConnection);
    }
}