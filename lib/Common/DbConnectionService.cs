using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace GLT
{
    public class DbConnectionService
    {
        private IServiceProvider _service;
        public ConfigService Config { get; }

        public DbConnectionService(IServiceProvider service)
        {
            this._service = service;
            this.Config = service.GetService<ConfigService>();
        }

        private IDbConnection OpenDbConnection(DbConnectionString connectionString)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public IDbConnection CoreDB_R(bool nonPooling = false)
        {
            if (nonPooling) return OpenDbConnection(Config.Db.CoreDB_R());
            return Config.Db.CoreDB_R().GetDbConnection(_service, OpenDbConnection);
        }
        public IDbConnection CoreDB_W(bool nonPooling = false)
        {
            if (nonPooling) return OpenDbConnection(Config.Db.CoreDB_W());
            return Config.Db.CoreDB_W().GetDbConnection(_service, OpenDbConnection);
        }
        public IDbTransaction CoreDB_WT(bool nonPooling = false) => CoreDB_W(nonPooling).BeginTransaction();

        public IDbConnection UserDB_R(int corpId, bool nonPooling = false)
        {
            if (nonPooling) return OpenDbConnection(Config.Db.CoreDB_R());
            return Config.Db.UserDB_R(corpId).GetDbConnection(_service, OpenDbConnection);
        }
        public IDbConnection UserDB_W(int corpId, bool nonPooling = false)
        {
            if (nonPooling) return OpenDbConnection(Config.Db.CoreDB_W());
            return Config.Db.UserDB_W(corpId).GetDbConnection(_service, OpenDbConnection);
        }
    }

    //internal class DbConnectionFactory : MiniProfiler.Integrations.IDbConnectionFactory
    //{
    //}
}
