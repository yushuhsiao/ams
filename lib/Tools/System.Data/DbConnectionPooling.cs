using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Threading;
//using System.Data.SqlClient;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Data
{
    [_DebuggerStepThrough]
    public static class DbConnectionPooling
    {
        static Dictionary<string, AsyncLocal<IDbConnection>> _dbConn = new Dictionary<string, AsyncLocal<IDbConnection>>();

        public static void ResetDbConnection(this DbConnectionString connectionString, IServiceProvider service)
        {
            var httpContext = service.GetService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                AsyncLocal<IDbConnection> state;
                lock (_dbConn)
                    if (_dbConn.TryGetValue(connectionString.Value, out state) == false)
                        return;
                using (var d = state.Value)
                    state.Value = null;
            }
            else
            {
                object key = typeof(_DbConn).AssemblyQualifiedName + connectionString.Value;
                if (httpContext.Items.TryGetValue(key, out var obj))
                    using (obj as _DbConn)
                        httpContext.Items.Remove(key);
            }
        }

        public static IDbConnection GetDbConnection(this DbConnectionString connectionString, IServiceProvider service, Func<DbConnectionString, IDbConnection> createDbConnection)
        {
            var httpContext = service.GetService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                AsyncLocal<IDbConnection> state;
                lock (_dbConn)
                    if (_dbConn.TryGetValue(connectionString.Value, out state) == false)
                        _dbConn[connectionString.Value] = state = new AsyncLocal<IDbConnection>();

                if (state.Value == null)
                    state.Value = createDbConnection(connectionString);
                return state.Value;
            }
            else
            {
                object key = typeof(_DbConn).AssemblyQualifiedName + connectionString.Value;
                if (httpContext.Items.TryGetValue(key, out var obj))
                    return (IDbConnection)obj;
                else
                {
                    var state = createDbConnection(connectionString);
                    httpContext.Items[key] = state;
                    httpContext.Response.RegisterForDispose(state);
                    return state;
                }
            }
        }

        private class _DbConn : IDbConnection
        {
            private IDbConnection _conn;
            private AsyncLocal<IDbConnection> _state;

            public _DbConn(IDbConnection conn, AsyncLocal<IDbConnection> state)
            {
                _conn = conn;
                _state = state;
            }

            void IDisposable.Dispose()
            {
                using (var d = this._conn)
                    this._conn = null;
                if (_state != null)
                    _state.Value = null;
            }

            string IDbConnection.ConnectionString
            {
                get => _conn.ConnectionString;
                set => _conn.ConnectionString = value;
            }

            int IDbConnection.ConnectionTimeout => _conn.ConnectionTimeout;

            string IDbConnection.Database => _conn.Database;

            ConnectionState IDbConnection.State => _conn.State;

            IDbTransaction IDbConnection.BeginTransaction() => _conn.BeginTransaction();

            IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il) => _conn.BeginTransaction(il);

            void IDbConnection.ChangeDatabase(string databaseName) => _conn.ChangeDatabase(databaseName);

            void IDbConnection.Close() => _conn.Close();

            IDbCommand IDbConnection.CreateCommand() => _conn.CreateCommand();

            void IDbConnection.Open() => _conn.Open();
        }


        //private static void _noop<T1, T2>(T1 t1, T2 t2) { }
        //private static T _noop<T1, T>(T1 t1) => default(T);

        //public static IServiceCollection AddDbConnectionPooling<TDbConnection>(this IServiceCollection services,
        //    Func<DbConnectionString, TDbConnection> createConnection,
        //    Func<IServiceProvider, object> getState,
        //    Action<object, IDisposable> registerForDispose)
        //    where TDbConnection : IDbConnection
        //{
        //    services.TryAddSingleton(new db<TDbConnection>.GetStateContainer()
        //    {
        //        CreateConnection = createConnection ?? _noop<DbConnectionString, TDbConnection>,
        //        GetState = getState ?? _noop<IServiceProvider, object>,
        //        RegisterForDispose = registerForDispose ?? _noop<object, IDisposable>
        //    });
        //    return services;
        //}

        //private static class db<TDbConnection> where TDbConnection : IDbConnection
        //{
        //    private class Pooling : List<PoolingConnection>, IDisposable
        //    {
        //        private static List<Pooling> _pooling1 = new List<Pooling>();
        //        private static Queue<Pooling> _pooling2 = new Queue<Pooling>();

        //        public object state { get; set; }

        //        public static Pooling GetInstance(object state, bool create, GetStateContainer getState)
        //        {
        //            if (state == null)
        //                return null;
        //            lock (_pooling1)
        //            {
        //                foreach (var tmp in _pooling1)
        //                    if (object.ReferenceEquals(tmp.state, state))
        //                        return tmp;

        //                if (create)
        //                {
        //                    Pooling p;
        //                    if (_pooling2.Count == 0)
        //                        p = new Pooling();
        //                    else
        //                        p = _pooling2.Dequeue();
        //                    p.state = state;
        //                    _pooling1.Add(p);
        //                    getState.RegisterForDispose(state, p);
        //                    return p;
        //                }
        //            }
        //            return null;
        //        }

        //        void IDisposable.Dispose()
        //        {
        //            lock (this)
        //            {
        //                this.state = null;
        //                while (this.Count > 0)
        //                    using (this[0])
        //                        this.RemoveAt(0);
        //            }
        //            lock (_pooling1)
        //            {
        //                _pooling1.RemoveAll(this);
        //                if (_pooling2.Contains(this) == false)
        //                    _pooling2.Enqueue(this);
        //            }
        //        }
        //    }

        //    private class PoolingConnection : IDbConnection
        //    {
        //        private Pooling _pooling;
        //        private IDbConnection _connection;

        //        public PoolingConnection(Pooling pooling, TDbConnection connection)
        //        {
        //            _pooling = pooling;
        //            _connection = connection;
        //        }

        //        public string ConnectionString
        //        {
        //            get => _connection.ConnectionString;
        //            set => _connection.ConnectionString = value;
        //        }

        //        public int ConnectionTimeout => _connection.ConnectionTimeout;

        //        public string Database => _connection.Database;

        //        public ConnectionState State => _connection.State;

        //        public IDbTransaction BeginTransaction() => _connection.BeginTransaction();

        //        public IDbTransaction BeginTransaction(IsolationLevel il) => _connection.BeginTransaction(il);

        //        public void ChangeDatabase(string databaseName) => _connection.ChangeDatabase(databaseName);

        //        public void Close() => _connection.Close();

        //        public IDbCommand CreateCommand() => _connection.CreateCommand();

        //        public void Open() => _connection.Open();

        //        public void Dispose()
        //        {
        //            if (_pooling != null)
        //            {
        //                lock (_pooling)
        //                    if (_pooling.Contains(this))
        //                        return;
        //            }
        //            _connection.Dispose();
        //        }
        //    }

        //    public class GetStateContainer
        //    {
        //        public Func<DbConnectionString, TDbConnection> CreateConnection { get; set; }
        //        public Func<IServiceProvider, object> GetState { get; set; }
        //        public Action<object, IDisposable> RegisterForDispose { get; set; }
        //    }

        //    private static TDbConnection CreateConnection(DbConnectionString cn, Func<DbConnectionString, TDbConnection> createConnection, GetStateContainer getState)
        //    {
        //        TDbConnection result = default(TDbConnection);
        //        if (createConnection != null)
        //        {
        //            try { result = createConnection(cn); }
        //            catch { }
        //        }
        //        if (result == null && getState != null)
        //        {
        //            try { result = getState.CreateConnection(cn); }
        //            catch { }
        //        }
        //        if (result != null)
        //        {
        //            try
        //            {
        //                if (result.State != ConnectionState.Open)
        //                    result.Open();
        //            }
        //            catch { }
        //        }
        //        return result;
        //    }

        //    public static IDbConnection OpenDbConnection(DbConnectionString cn, IServiceProvider services, Func<DbConnectionString, TDbConnection> createConnection,  object state)
        //    {
        //        if (services == null)
        //            return null;

        //        GetStateContainer getState = services.GetService<GetStateContainer>();
        //        state = state ?? getState.GetState(services);
        //        if (state == null)
        //            return CreateConnection(cn, createConnection, getState);

        //        var p = Pooling.GetInstance(state, true, getState);
        //        lock (p)
        //        {
        //            foreach (var c in p)
        //                if (c.ConnectionString == cn)
        //                    return c;

        //            TDbConnection connection = CreateConnection(cn, createConnection, getState);
        //            var result = new PoolingConnection(p, connection);
        //            p.Add(result);
        //            return result;
        //        }
        //    }

        //    public static void Release(object state)
        //    {
        //        using (var p = Pooling.GetInstance(state, false, null))
        //        {
        //        }
        //    }
        //}

        ////public static IDbConnection OpenDbConnection<TDbConnection>(this DbConnectionString cn,
        ////    Func<DbConnectionString, TDbConnection> createConnection,
        ////    IServiceProvider services,
        ////    object state = null)
        ////    where TDbConnection : IDbConnection
        ////    => db<TDbConnection>.OpenDbConnection(cn, createConnection, services, state);

        ////public static IDbConnection OpenDbConnection<TDbConnection>(this DbConnectionString cn,
        ////    IServiceProvider services,
        ////    object state = null)
        ////    where TDbConnection : IDbConnection
        ////    => db<TDbConnection>.OpenDbConnection(cn, null, services, state);


        ////public static IDbConnection OpenDbConnection(this DbConnectionString cn,
        ////    IServiceProvider services,
        ////    object state = null)
        ////    => db<SqlConnection>.OpenDbConnection(cn, services, null, state);

        //public static void Release<TDbConnection>(object state) where TDbConnection : IDbConnection
        //    => db<TDbConnection>.Release(state);

    }
}