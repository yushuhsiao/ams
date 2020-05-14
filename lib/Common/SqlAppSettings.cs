using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using Dapper;
using StackExchange.Redis;
using System.Runtime.CompilerServices;

namespace GLT
{
    public static class SqlAppSettingsExtensions
    {
        public static IConfigurationBuilder AddSqlAppSettings(this IConfigurationBuilder builder)
        {
            builder.Add(new SqlAppSettingsConfigurationSource());
            return builder;
        }

        public static TValue GetValue<TValue>(this IConfiguration configuration, int corpId, [CallerMemberName] string name = null, params object[] index)
            => AppSettingBinder.GetValue<TValue>(configuration, name, corpId);
    }
    public class SqlAppSettingsConfigurationSource : IConfigurationSource
    {
        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder) 
            => new SqlAppSettingsConfigurationProvider();
    }

    public class SqlAppSettingsConfigurationProvider : AppSettingBinder.Provider
    {
        private IServiceProvider _service;
        //private DbConnectionService _dbCnnectionService;
        private DbCache<Entity.Config> _dbCache;

        public override void OnInit(IServiceProvider service)
        {
            var obj = Interlocked.CompareExchange(ref _service, service, null);
            if (obj == null) return;
            //Interlocked.Exchange(ref _dbCnnectionService, _service.GetService<DbConnectionService>());
            Interlocked.Exchange(ref _dbCache, _service.GetDbCache<Entity.Config>(ReadData));
        }

        private IEnumerable<Entity.Config> ReadData(DbCache<Entity.Config>.Entry sender, Entity.Config[] oldValue)
        {
            var _service = Interlocked.CompareExchange(ref this._service, null, null);
            if (_service == null)
                return null;
            var _dbCnnectionService = _service.GetService<DbConnectionService>();
            string sql = $"select * from {TableName<Entity.Config>.Value} where CorpId = {sender.Index}";
            using (var conn = _dbCnnectionService.CoreDB_R(nonPooling: true))
            {
                var rows = conn.Query<Entity.Config>(sql);
                foreach (var row in rows)
                {
                    if (row.CorpId == 0)
                        row.Key = $"{row.Key1}:{row.Key2}";
                    else
                        row.Key = $"{row.Key1}:{row.Key2}:{row.CorpId}";
                }
                return rows;
            }
        }

        //public bool OnGetValue<TValue>(string section, string key, out TValue value, params object[] index)
        //{
        //    var service = Interlocked.CompareExchange(ref _service, null, null);
        //    if (service != null)
        //    {
        //    }
        //    value = default;
        //    return false;
        //}

        //public bool OnGetValue(string section, string key, out string value, params object[] index)
        //{
        //    var _dbCacahe = Interlocked.CompareExchange(ref this._dbCache, null, null);
        //    if (_dbCacahe != null)
        //    {
        //        int corpId = 0;
        //        for (int i = 0; i < index.Length; i++)
        //            if (index[i] is int)
        //                corpId = (int)index[i];
        //        var values = _dbCache.GetValues(index: corpId);
        //        for (int i = 0; i < values.Length; i++)
        //        {
        //            var n = values[i];
        //            //if (n.CorpId != corpId) continue;
        //            if (n.Key1.IsNotEquals(section)) continue;
        //            if (n.Key2.IsNotEquals(key)) continue;
        //            value = n.Value;
        //            return true;
        //        }
        //    }
        //    value = null;
        //    return false;
        //}

        public override bool TryGet(string key, out string value)
        {
            var _dbCacahe = Interlocked.CompareExchange(ref this._dbCache, null, null);
            if (_dbCacahe == null)
                return base.TryGet(key, out value);
            var values = _dbCacahe.GetValues();
            for (int i = 0; i < values.Length; i++)
            {
                var row = values[i];
                if (string.Compare(row.Key, key, true) == 0)
                {
                    value = row.Value;
                    return true;
                }
            }
            return base.TryGet(key, out value);
        }
    }
}



    //private class _ConfigurationProvider
    //{
    //    private IServiceProvider _service;
    //    private IConfiguration _config;
    //    private ILogger _logger;
    //    private DbCache<Entity.Config> _dbcache;
    //    private Dictionary<string, string> _data = new Dictionary<string, string>();
    //    private TimeCounter _timer1 = new TimeCounter(false);
    //    private TimeCounter _timer2 = new TimeCounter(false);
    //    private int _read_count = 0;
    //    private List<int> _busy = new List<int>();

    //    public _ConfigurationProvider(IServiceProvider service, SqlAppSettingsConfigurationProvider src)
    //    {
    //        src.Instance2 = this;
    //        _service = service;
    //        _config = service.GetService<IConfiguration<_ConfigurationProvider>>();
    //        _logger = service.GetService<ILogger<_ConfigurationProvider>>();
    //        _dbcache = service.GetDbCache<Entity.Config>(ReadData);
    //    }

    //    private IEnumerable<Entity.Config> ReadData(DbCache<Entity.Config>.Entry sender, Entity.Config[] oldValue)
    //    {
    //        yield break;
    //    }

    //    [AppSetting(SectionName = "Config", Key = "ExpireTime"), DefaultValue(5 * 60 * 1000)]
    //    private double ExpireTime => Math.Max(1000, _config.GetValue<double>());

    //    [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
    //    private DbConnectionString CoreDB_R => _config.GetValue<DbConnectionString>();

    //    public bool TryGet(string key, out string value)
    //    {
    //        if (Monitor.TryEnter(this))
    //        {
    //            int t = Thread.CurrentThread.ManagedThreadId;
    //            try
    //            {
    //                if (_busy.Contains(t))
    //                    return _data.TryGetValue(key, out value);
    //                _busy.Add(t);

    //                try
    //                {
    //                    if (_read_count == 0)
    //                        return this.TryGet(true, key, out value);

    //                    if (_timer1.IsTimeout(ExpireTime))
    //                        return this.TryGet(false, key, out value);

    //                    if (_dbcache.GetValues(out var tmp)) // redis has change
    //                        return this.TryGet(false, key, out value);

    //                    return _data.TryGetValue(key, out value);
    //                }
    //                finally { _busy.RemoveAll(t); }
    //            }
    //            finally { Monitor.Exit(this); }
    //        }
    //        value = null;
    //        return false;
    //    }

    //    private bool TryGet(bool force, string key, out string value)
    //    {
    //        if (force || _timer2.IsTimeout(100, true))
    //        {
    //            try
    //            {
    //                string sql = $"select * from {TableName<Entity.Config>.Value} where CorpId = 0";
    //                using (IDbConnection conn = CoreDB_R.OpenDbConnection(
    //                    _connStr => new SqlConnection(_connStr),
    //                    _service,
    //                    null))
    //                {
    //                    var rows = conn.Query<Entity.Config>(sql);
    //                    _data.Clear();
    //                    foreach (var row in rows)
    //                        _data[$"{row.Key1}:{row.Key2}"] = row.Value;
    //                    _read_count++;
    //                    _timer1.Reset();
    //                    _timer2.Reset();
    //                    _logger.LogInformation("Reload configure...");
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                _logger.LogError(ex, ex.Message);
    //            }
    //        }
    //        return _data.TryGetValue(key, out value);
    //    }

    //    public bool TryGet2(string key, out string value)
    //    {
    //        value = null;
    //        return false;
    //    }
    //}

    //void AppSettingBinder._IConfigurationProvider.Init(IServiceProvider service)
    //{
    //    if (Instance is SqlConfig) return;
    //    new SqlConfig(service, this);
    //    //if (Instance == null)
    //    //    new _ConfigurationProvider(service, this);
    //}

    //private class _SqlConfig
    //{
    //    public virtual bool GetValue(string key, out string value) { value = null; return false; }
    //}

    //private class SqlConfig : _SqlConfig
    //{
    //    private DataService _service;
    //    private DbCache<Entity.Config> _dbCache;

    //    public SqlConfig(IServiceProvider service, SqlAppSettingsConfigurationProvider src)
    //    {
    //        src.Instance = this;
    //        _service = service.GetService<DataService>();
    //        //_dbCache = service.GetDbCache<Entity.Config>(ReadData);
    //    }

    //    private IEnumerable<Entity.Config> ReadData(DbCache<Entity.Config>.Entry sender, Entity.Config[] oldValue)
    //    {
    //        string sql = $"select * from {TableName<Entity.Config>.Value} where CorpId = {sender.Index}";
    //        using (IDbConnection conn = _service._CoreDB_R().OpenDbConnection(
    //            _connStr => new SqlConnection(_connStr),
    //            _service,
    //            null))
    //        {
    //            return conn.Query<Entity.Config>(sql);
    //        }
    //    }

    //    public override bool GetValue(string key, out string value)
    //    {
    //        if (_dbCache != null)
    //        {
    //            var data = _dbCache.GetValues();
    //        }
    //        return base.GetValue(key, out value);
    //    }
    //}

    //_ConfigurationProvider _instance2;
    //_ConfigurationProvider Instance2
    //{
    //    get => Interlocked.CompareExchange(ref _instance2, null, null);
    //    set => Interlocked.Exchange(ref _instance2, value);
    //}

    //_SqlConfig _instance = new _SqlConfig();
    //_SqlConfig Instance
    //{
    //    get => Interlocked.CompareExchange(ref _instance, null, null);
    //    set => Interlocked.Exchange(ref _instance, value);
    //}

    //public override bool TryGet(string key, out string value)
    //{
    //    var obj = Instance;
    //    if (obj == null)
    //        return base.TryGet(key, out value);
    //    else
    //        return obj.GetValue(key, out value);
    //}
    //public class SqlAppSettingsConfigurationProvider : ConfigurationProvider//IConfigurationProvider
    //{
    //    private DataService _services;
    //    private IConfiguration<SqlAppSettingsConfigurationProvider> _config;
    //    private ILogger _logger;
    //    private TimeCounter _timer = new TimeCounter(false);
    //    private object _sync = new object();

    //    public SqlAppSettingsConfigurationProvider() { }

    //    internal static void Init(IServiceProvider services)
    //    {
    //        IConfigurationRoot config = services.GetService<IConfiguration>() as IConfigurationRoot;
    //        foreach (var n in config?.Providers)
    //        {
    //            if (n.TryCast(out SqlAppSettingsConfigurationProvider obj))
    //            {
    //                obj._services = services.GetService<DataService>();
    //                obj._config = services.GetService<IConfiguration<SqlAppSettingsConfigurationProvider>>();
    //                obj._logger = services.GetService<ILogger<SqlAppSettingsConfigurationProvider>>();
    //            }
    //        }
    //    }

    //    [DebuggerHidden]
    //    [AppSetting(SectionName = "Config", Key = "ExpireTime"), DefaultValue(5 * 60 * 1000)]
    //    public double ExpireTime
    //    {
    //        //[DebuggerStepThrough]
    //        get => _config.GetValue<double>();
    //    }

    //    private bool _Reload()
    //    {
    //        if (_config == null)
    //            return false;

    //        double timeout = ExpireTime;

    //        lock (_sync)
    //        {
    //            if (base.Data.Count == 0)
    //                goto _read;

    //            if (_timer.IsTimeout(timeout))
    //                goto _read;

    //            // ToDo: check redis
    //        }
    //        return false;
    //    _read:
    //        return _Read();
    //    }

    //    private bool _Read()
    //    {
    //        string cn = _services._CoreDB_R();// ConnectionString;
    //        try
    //        {
    //            lock (_sync)
    //            {
    //                List<Entity.Config> tmp = SqlCmd.ToList<Entity.Config>(cn, _services, $"select * from {TableName<Entity.Config>.Value} where CorpId = 0");

    //                base.Data.Clear();
    //                foreach (var row in tmp)
    //                    base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
    //                _timer.Reset();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.Log(LogLevel.Error, ex, ex.Message);
    //            return false;
    //        }
    //        return true;
    //    }

    //    //private void _Reload2()
    //    //{
    //    //    if (_config == null)
    //    //        return;

    //    //    double timeout = ExpireTime;
    //    //    lock (_sync)
    //    //    {
    //    //        if (base.Data.Count == 0)
    //    //            goto _load;

    //    //        if (_time.IsTimeout(timeout))
    //    //            goto _load;

    //    //        // ToDo: check redis
    //    //    }

    //    //    return;

    //    //_load:
    //    //    string cn = _services._CoreDB_R;// ConnectionString;
    //    //    try
    //    //    {
    //    //        List<Data.Config> tmp;
    //    //        using (SqlCmd sqlcmd = new SqlCmd(cn, _services))
    //    //            tmp = sqlcmd.ToList<Data.Config>($"select * from {TableName<Data.Config>.Value} where CorpId = 0");
    //    //        lock (_sync)
    //    //        {
    //    //            base.Data.Clear();
    //    //            foreach (var row in tmp)
    //    //                base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
    //    //            _time.Reset();
    //    //        }
    //    //        //lock (_sync)
    //    //        //{
    //    //        //    using (SqlCmd sqlcmd = new SqlCmd(cn, _services))
    //    //        //    {
    //    //        //        base.Data.Clear();
    //    //        //        foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach($"select * from {TableName<Data.Config>.Value} where CorpId = 0"))
    //    //        //        {
    //    //        //            Data.Config row = r.ToObject<Data.Config>();
    //    //        //            base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
    //    //        //        }
    //    //        //        _time.Reset();
    //    //        //    }
    //    //        //}
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        _logger.Log(LogLevel.Error, ex, ex.Message);
    //    //    }
    //    //}

    //    private List<int> _TryGet_busy = new List<int>();
    //    public override bool TryGet(string key, out string value)
    //    {
    //        int t = Thread.CurrentThread.ManagedThreadId;
    //        lock (_TryGet_busy)
    //        {
    //            if (_TryGet_busy.Contains(t))
    //            {
    //                value = null;
    //                return false;
    //            }
    //            _TryGet_busy.Add(t);
    //        }
    //        try
    //        {
    //            _Reload();
    //            lock (_sync)
    //                return base.TryGet(key, out value);
    //        }
    //        finally
    //        {
    //            lock (_TryGet_busy)
    //            {
    //                _TryGet_busy.Remove(t);
    //            }
    //        }
    //    }
    //}
