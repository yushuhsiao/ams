using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace GLT
{
    namespace GLT
    {
        public static class DbCacheExtensions
        {
            [DebuggerStepThrough]
            public static DataService DataService(this IServiceProvider services) => services.GetService<DataService>();
        }
    }

    public class DataService : IServiceProvider
    {
        private IServiceProvider _service;
        private Dictionary<Type, IDataService> _instances = new Dictionary<Type, IDataService>();

        public DataService(IServiceProvider service)
        {
            _service = service;
        }

        private T GetInstance<T>() where T : IDataService
        {
            for (; ; Thread.Sleep(1))
            {
                if (Monitor.TryEnter(_instances))
                {
                    try
                    {
                        if (_instances.TryGetValue(typeof(T), out var tmp))
                        {
                            if (tmp is T result)
                                return result;
                            continue;
                        }
                        else
                        {
                            _instances[typeof(T)] = null;
                        }
                    }
                    finally
                    {
                        Monitor.Exit(_instances);
                    }
                    T obj = _service.CreateInstance<T>();
                    lock (_instances)
                        _instances[typeof(T)] = obj;
                    return obj;
                }
            }
        }

        object IServiceProvider.GetService(Type serviceType) => _service.GetService(serviceType);
    
        public ConfigService Config => _service.GetService<ConfigService>();

        public DbConnectionService Db => _service.GetService<DbConnectionService>();

        public AclDefineProvider Acl => GetInstance<AclDefineProvider>();

        public CorpInfoProvider Corps => GetInstance<CorpInfoProvider>();
    }
}
