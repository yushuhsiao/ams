using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace GLT
{
    public class DbCache
    {
        private IServiceProvider _services;
        private ILogger<DbCache> _logger;
        private IConfiguration<DbCache> _config;

        public DbCache(IServiceProvider services)
        {
            this._services = services;
            this._config = services.GetRequiredService<IConfiguration<DbCache>>();
        }

        private ILogger GetLogger()
        {
            var logger = Interlocked.CompareExchange(ref _logger, null, null);
            if (logger != null)
                return logger;
            logger = _services.GetService<ILogger<DbCache>>();
            Interlocked.Exchange(ref _logger, logger);
            return logger;
        }

        #region Items, Add, Get

        private List<IDbCache> _items1 = new List<IDbCache>();
        private IDbCache[] _items2 = new IDbCache[0];

        internal void Add<TValue>(DbCache<TValue> item)
        {
            lock (_items1)
            {
                _items1.TryAdd(item);
                _items2 = Interlocked.Exchange(ref _items2, _items1.ToArray());
            }
        }

        public DbCache<TValue> Get<TValue>(DbCache<TValue>.ReadDataHandler readData = null, string dbName = _Consts.db.CoreDB, string tableName = null)
        {
            lock (_items1)
            {
                var _items = Interlocked.CompareExchange(ref _items2, null, null);
                DbCache<TValue> item;
                for (int i = 0; i < _items.Length; i++)
                {
                    item = _items[i] as DbCache<TValue>;
                    if (item != null)
                        return item;
                }
                item = new DbCache<TValue>(_services, this, dbName, tableName) { ReadData = readData };
                this.Add(item);
                return item;
            }
        }

        #endregion

        #region Global Events

        //[RedisAction(Channel = _Consts.Redis.Channels.AppControl, Name = nameof(ServerCommands.PurgeCache), Instance = InstanceFlags.FromService)]
        public void PurgeCache(params string[] cacheTypes)
        {
            var _items = Interlocked.CompareExchange(ref _items2, null, null);
            for (int i = 0, n = _items.Length; i < n; i++)
            {
                IDbCache obj = _items[i];
                if (cacheTypes.Contains(obj.TableName))
                    obj.PurgeCache();
            }
            GetLogger().Log(LogLevel.Information, 0, "PurgeCache");
        }

        #endregion

        #region Redis

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.Main), DefaultValue(_Consts.Redis.DefaultValue)]
        //public string Redis_Main() => GetConfig().GetValue<string>();
        //[AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.Redis.Main), DefaultValue(_Consts.Redis.DefaultValue)]
        //public string Redis_Main() => _config.GetValue<string>();

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.TableVer), DefaultValue(1)]
        //public int Redis_TableVer() => GetConfig().GetValue<int>();
        //[AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.Redis.TableVer), DefaultValue(_Consts.Redis.TableVer_DefaultValue)]
        public string Redis_TableVer() => _config.GetValue<string>();

        internal bool RedisGetVersion(IDbCacheEntry entry, out long value)
        {
            using (var redis = _services.GetRedisConnection(Redis_TableVer()))
            {
                RedisValue n = redis.StringGet(entry.RedisKey);
                if (n.HasValue)
                    return n.TryParse(out value);
            }
            value = default;
            return false;
        }

        internal bool RedisSetVersion(IDbCacheEntry entry, long? value)
        {
            try
            {
                using (var redis = _services.GetRedisConnection(Redis_TableVer()))
                {
                    bool result;
                    if (value.HasValue)
                        result = redis.StringSet(entry.RedisKey, value.Value.ToString(), expiry: entry.Parent.RedisKeyExpire);
                    else
                        result = redis.KeyDelete(entry.RedisKey);
                }
            }
            catch(Exception ex)
            {
                GetLogger().LogError(ex.Message);
            }
            return false;
        }

        //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        //public class UpdateMessage2 : RedisMessage
        //{
        //    [JsonProperty]
        //    public string Name { get; set; }

        //    [JsonProperty]
        //    public int Index { get; set; }

        //    [JsonProperty]
        //    public long? Version { get; set; }
        //}

        //private static readonly RedisMessage.Pool<UpdateMessage2> _msg = new RedisMessage.Pool<UpdateMessage2>();

        internal long RedisUpdateVersion(IDbCacheEntry entry, long? value)
        {
            using (var redis = _services.GetRedisConnection(Redis_TableVer()))
            {
                //foreach (var m in _msg.Publish(redis, "TableVer"))
                //{
                //    m.Name = entry.Parent.Name;
                //    m.Index = entry.Index;
                //    m.Version = value;
                //}
            }
            return 0;
            //try
            //{
            //    var db = _redis.GetDatabase(null, Redis_TableVer);
            //    foreach (var m in _msg.Publish(db, _Consts.Redis.Channels.TableVer))
            //    {
            //        m.Name = entry.Parent.Name;
            //        m.Index = entry.Index;
            //        m.Version = value;
            //    }
            //    _redis.SetIdle();
            //}
            //catch (Exception ex)
            //{
            //    _redis.OnError(GetLogger(), ex, $"Redis Publish: {entry.RedisKey}, {value}"); //_logger.LogError(ex, "Redis StringSet : {0}, {1}", entry.RedisKey, value);
            //}
            //return 0;
        }

        #endregion

        #region sql

        //private RedisDatabase _redis;

        //private object sync_redis = new object();
        private object sync_sql = new object();
        private IDbConnection dbConn_r = null;
        private IDbConnection dbConn_w = null;

        //private RedisSubscriber<UpdateMessage2> _redis;
        //private ConnectionMultiplexer _mux;
        //private IDatabase _db;
        //private ISubscriber _sub;

        //private ConnectionMultiplexer GetMultiplexer()
        //{
        //    if (_mux == null)
        //    {
        //        string configuration = Redis_Main();
        //        _mux = ConnectionMultiplexer.Connect(configuration);
        //    }
        //    return _mux;
        //}

        //private IDatabase GetDatabase()
        //{
        //    if (_db == null)
        //    {
        //        var config = this.Redis_TableVer();
        //        _db = ConnectionMultiplexer.Connect(config).GetDatabase();
        //        //int dbindex = Redis_TableVer();
        //        //_db = GetMultiplexer().GetDatabase(dbindex);
        //        //_db = _redis.GetDatabase();
        //    }
        //    return _db;
        //}

        //private void OnRedisError(Exception ex, string msg, params object[] args)
        //{
        //    _logger.Log(LogLevel.Error, 0, msg, args);
        //    using (_db?.Multiplexer)
        //        _db = null;
        //}

        //private void ResetRedis(string msg, params object[] args)
        //{
        //    _redis.Reset();
        //    _logger.Log(LogLevel.Error, 0, msg, args);
        //    _db = null;
        //}

        internal bool SqlGetVersion(IDbCacheEntry entry, out long value)
        {
            var cn = _services.GetService<DbConnectionService>().Config.CoreDB_R();
            return sql_exec("TableVer_get", cn, ref dbConn_r, entry, out value);
        }

        internal bool SqlSetVersion(IDbCacheEntry entry, out long value)
        {
            var cn = _services.GetService<DbConnectionService>().Config.CoreDB_W();
            return sql_exec("TableVer_set", cn, ref dbConn_w, entry, out value);
        }

        private bool sql_exec(string sp, DbConnectionString cn, ref IDbConnection conn, IDbCacheEntry entry, out long value)
        {
            for (int r = 0; r < 3; r++)
            {
                try
                {
                    lock (sync_sql)
                    {
                        if (conn == null)
                            conn = _services.GetService<DbConnectionService>().OpenDbConnection(cn);

                        var param = new
                        {
                            db = sql_DbName(entry.Parent.DbName),
                            table = sql_TableName(entry.Parent.TableName),
                            index = entry.Index
                        };
                        object tmp1;

                        try
                        {
                            IDbTransaction tran = null;
                            bool isWrite = object.ReferenceEquals(conn, this.dbConn_w);
                            if (isWrite) tran = conn.BeginTransaction();
                            using (tran)
                            {
                                tmp1 = conn.ExecuteScalar(sp, param, transaction: tran, commandType: CommandType.StoredProcedure);
                                tran?.Commit();
                            }
                        }
                        catch
                        {
                            using (var _conn = conn)
                            {
                                conn = null;
                                throw;
                            }
                        }
                        if (SqlTimeStamp.Create(tmp1, out SqlTimeStamp tmp2))
                        {
                            value = tmp2;
                            return true;
                        }
                        else
                        {
                            value = 0;
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GetLogger().LogError(ex, ex.Message); //_logger.LogError(ex, null);
                }
            }
            value = 0;
            return false;
        }

        [DebuggerStepThrough]
        private static string sql_DbName(string name)
        {
            if (name.Length > 20)
                return name.Substring(20);
            return name;
        }

        [DebuggerStepThrough]
        private static string sql_TableName(string name)
        {
            if (name.Length > 30)
                return name.Substring(30);
            return name;
        }

        #endregion

        [DebuggerStepThrough]
        internal bool ReadData<TValue>(DbCache<TValue>.Entry sender, TValue[] values, out TValue[] result, DbCache<TValue>.ReadDataHandler readData)
        {
            try
            {
                result = null;
                IEnumerable<TValue> list = readData?.Invoke(sender, values);
                if (list != null)
                {
                    if (list is TValue[])
                        result = (TValue[])list;
                    if (list is List<TValue>)
                        result = ((List<TValue>)list).ToArray();
                    else
                    {
                        List<TValue> tmp = null;
                        foreach (var n in list)
                        {
                            if (tmp == null)
                                tmp = new List<TValue>();
                            tmp.Add(n);
                        }
                        result = tmp?.ToArray();
                    }
                }
                return result != null;
            }
            catch (Exception ex)
            {
                GetLogger().LogError(ex, ex.Message);
                result = default;
                return false;
            }
        }
    }

    public static class DbCacheExtensions
    {
        public static DbCache<TValue> GetDbCache<TValue>(this IServiceProvider services, DbCache<TValue>.ReadDataHandler readData = null, string name = null)
            => services.GetService<DbCache>().Get(readData, name);
    }

    internal interface IDbCache
    {
        string DbName { get; }
        string TableName { get; }
        void PurgeCache();
        TimeSpan RedisKeyExpire { get; }
    }

    internal interface IDbCacheEntry
    {
        IDbCache Parent { get; }
        int Index { get; }
        RedisKey RedisKey { get; }
    }

    public sealed class DbCache<TValue> : IDbCache
    {
        public sealed partial class Entry : IDbCacheEntry
        {
            public DbCache<TValue> Parent { get; }
            public int Index { get; }
            private RedisKey RedisKey;
            private long _version = -1;
            public long Version
            {
                get => Interlocked.Read(ref _version);
                set => Interlocked.Exchange(ref _version, value);
            }

            RedisKey IDbCacheEntry.RedisKey => RedisKey;
            IDbCache IDbCacheEntry.Parent => Parent;

            internal Entry(DbCache<TValue> parent, int index)
            {
                this.Parent = parent;
                this.Index = index;
                this.SetName();
            }

            private TValue[] _values;

            //private ReaderWriterLockSlim sync1 = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            private TimeCounter timer1 = new TimeCounter(false);    // timeout
            private TimeCounter timer2 = new TimeCounter(false);    // timeout for check version from redis


            //bool isBusy;
            //bool isReading;
            object _busy;
            object _reading;
            private bool IsReading
            {
                get => Interlocked.CompareExchange(ref _reading, null, null) != null;
                set => Interlocked.Exchange(ref _reading, value ? this : null);
            }

            public TValue GetFirstValue(ReadDataHandler readData = null)
            {
                GetValues(out var values, readData);
                if (values == null) return default(TValue);
                if (values.Length == 0) return default(TValue);
                return values[0];
            }

            public TValue[] GetValues(ReadDataHandler readData = null)
            {
                GetValues(out var values, readData);
                return values;
            }

            public bool GetValues(out TValue[] result, ReadDataHandler readData = null)
            {
                TValue[] oldValue = Interlocked.CompareExchange(ref this._values, null, null);
                result = oldValue ?? _Empty<TValue>.Array;
                if (Monitor.TryEnter(timer1) == false)
                    return false;

                try
                {
                    var isBusy = Interlocked.CompareExchange(ref this._busy, this, null) != null;
                    if (isBusy)
                        return false;

                    try
                    {
                        if (oldValue == null)
                        {
                            oldValue = new TValue[0];
                            goto _read;
                        }

                        if (timer1.IsTimeout(Parent.Timeout, reset: false))
                            goto _read;

                        if (timer2.IsTimeout(Parent.RedisInterval, false))
                        {
                            if (!Parent.Root.RedisGetVersion(this, out long version))
                                goto _read;

                            if (version != this.Version)
                                goto _read;

                            timer2.Reset();
                        }
                        return false;

                    _read:
                        try
                        {
                            this.IsReading = true;
                            if (!Parent.Root.ReadData(this, oldValue, out TValue[] newValue, readData ?? Parent.ReadData/*, readData2 ?? Parent.ReadData2*/))
                                return false;
                            result = newValue;
                            Interlocked.Exchange(ref this._values, newValue);
                            Parent.Root.SqlGetVersion(this, out long version);
                            this.Version = version;
                            Parent.Root.RedisSetVersion(this, version);
                            oldValue = oldValue ?? _Empty<TValue>.Array;
                            for (int i = 0; i < oldValue.Length; i++)
                            {
                                TValue n = oldValue[i];
                                oldValue[i] = default(TValue);
                                if (!newValue.Contains(n))
                                    using (n as IDisposable)
                                        continue;
                            }
                            timer2.Reset();
                            timer1.Reset();
                            return true;
                        }
                        finally
                        {
                            this.IsReading = false;
                        }
                    }
                    finally
                    {
                        Interlocked.Exchange(ref this._busy, null);
                    }
                }
                finally
                {
                    Monitor.Exit(timer1);
                }
            }

            public void ClearValues() => timer1.ClearTicks();

            public void UpdateVersion()
            {
                Parent.Root.SqlSetVersion(this, out long ver);
                if (!this.IsReading)
                {
                    this.Version = 0;
                    Parent.Root.RedisSetVersion(this, null);
                    timer2.ClearTicks();
                    timer1.ClearTicks();
                }
            }
        }

        public DbCache Root { get; }
        public Entry Default => this[0];
        //private IServiceProvider _services;

        internal DbCache(IServiceProvider services, DbCache dbCache, string dbName, string tableName)
        {
            //this._services = services;
            //this.__logger = services.GetRequiredService<ILogger<DbCache<TValue>>>();
            this.Root = dbCache;
            this.Root.Add(this);
            Interlocked.Exchange(ref this._entrys2, new Entry[] { new Entry(this, 0) });
            this.SetName(dbName, tableName);
        }

        private readonly object _sync = new object();
        private Entry[] _entrys2;
        public Entry this[int index]
        {
            get
            {
                var entrys = Interlocked.CompareExchange(ref this._entrys2, null, null);
                for (int i = 0, n = entrys.Length; i < n; i++)
                {
                    var entry = entrys[i];
                    if (entry.Index == index)
                        return entry;
                }
                lock (_sync)
                {
                    var entry = new Entry(this, index);
                    Interlocked.Exchange(ref this._entrys2, entrys.Add(entry));
                    return entry;
                }
                //if (index == 0) return this.Default;
                //return _entrys.GetValue(index, () => new Entry((TDbCache)this, index), true);
                //lock (_entrys)
                //    if (_entrys.TryGetValue(index, out Entry result))
                //        return result;
                //    else
                //        return _entrys[index] = new Entry(this, index);
            }   //
        }

        public string DbName { get; private set; }
        public string TableName { get; private set; }

        //private ILogger _logger;
        //private ILogger GetLogger()
        //{
        //    var logger = Interlocked.CompareExchange(ref _logger, null, null);
        //    if (logger != null)
        //        return logger;
        //    logger = _services.GetService<ILogger<DbCache<TValue>>>();
        //    Interlocked.Exchange(ref _logger, logger);
        //    return logger;
        //}

        #region SetName

        private void SetName(string dbName, string tableName)
        {
            var attr = TableName<TValue>._;
            this.DbName = dbName.Trim(true) ?? attr?.Database?.Trim(true) ?? _Consts.db.CoreDB;
            this.TableName = tableName.Trim(true) ?? attr?.TableName?.Trim(true) ?? typeof(TValue).Name;
            var entrys = Interlocked.CompareExchange(ref this._entrys2, null, null);
            for (int i = 0, n = entrys.Length; i < n; i++)
                entrys[i].SetName();
        }
        partial class Entry
        {
            internal void SetName()
            {
                this.RedisKey = $"{Parent.DbName}.{Parent.TableName}.{Index}";
            }
        }

        #endregion

        private double _Timeout = TimeSpan.FromMinutes(30).TotalMilliseconds;
        public double Timeout
        {
            get => Interlocked.CompareExchange(ref _Timeout, 0, 0).Max(600000);
            set => Interlocked.Exchange(ref _Timeout, value);
        }

        private double _RedisInterval = 3000;
        public double RedisInterval
        {
            get => Interlocked.CompareExchange(ref _RedisInterval, 0, 0).Max(500);
            set => Interlocked.Exchange(ref _RedisInterval, value);
        }

        private Tuple<TimeSpan> _RedisKeyExpire = Tuple.Create(TimeSpan.FromMinutes(10));
        public TimeSpan RedisKeyExpire
        {
            get => Interlocked.CompareExchange(ref _RedisKeyExpire, null, null).Item1;
            set => Interlocked.Exchange(ref _RedisKeyExpire, Tuple.Create(value));
        }

        //public SqlConfig SqlConfig() => _dbCache.GetConfig().Root;

        public delegate IEnumerable<TValue> ReadDataHandler(Entry sender, TValue[] oldValue);

        public ReadDataHandler ReadData { get; set; }
        public event ReadDataHandler ReadDataEvent
        {
            add => ReadData = value;
            remove => ReadData = null;
        }

        public void PurgeCache()
        {
            var entrys = Interlocked.CompareExchange(ref this._entrys2, null, null);
            for (int i = 0, n = entrys.Length; i < n; i++)
                entrys[i].ClearValues();
        }

        [DebuggerStepThrough]
        public TValue GetFirstValue(ReadDataHandler readData = null, int index = 0) => this[index].GetFirstValue(readData);

        [DebuggerStepThrough]
        public TValue[] GetValues(ReadDataHandler readData = null, int index = 0) => this[index].GetValues(readData);

        /// <returns>value has change</returns>
        [DebuggerStepThrough]
        public bool GetValues(out TValue[] result, ReadDataHandler readData = null, int index = 0) => this[index].GetValues(out result, readData);

        [DebuggerStepThrough]
        public void ClearValues(int index = 0) => this[index].ClearValues();

        [DebuggerStepThrough]
        public void UpdateVersion(int index = 0) => this[index].UpdateVersion();

        [DebuggerStepThrough]
        public void UpdateVersion(IEnumerable<int> indexes)
        {
            foreach (int index in indexes)
                this[index].UpdateVersion();
        }

        //#region util

        //[DebuggerStepThrough]
        //void writelog(string message, params object[] args)
        //{
        //    _logger.LogInformation(message, args);
        //}

        //[DebuggerStepThrough]
        //static object[] _args(object arg)
        //{
        //    if (arg == null) return _null.objects;
        //    return new object[] { arg };
        //}

        //#endregion
    }

    //public sealed class DbCache<TValue> : DbCache<DbCache<TValue>, TValue>
    //{
    //    public DbCache(DbCache provider) : base(provider) { }
    //    protected override IEnumerable<TValue> OnReadData(Entry sender, List<TValue> values) => null;
    //}
}