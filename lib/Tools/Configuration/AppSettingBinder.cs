﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Configuration
{
    public interface IConfiguration<TCallerType> : IConfiguration { }

    public static class AppSettingBinder
    {
        public static IServiceCollection AddConfigurationBinder(this IServiceCollection service)
        {
            //service.AddSingleton(typeof(IConfiguration<>), typeof(_Binder<>));
            service.TryAddSingleton(typeof(IConfiguration<>), typeof(_Binder<>));
            return service;
        }

        public static IConfiguration GetConfiguration(this IServiceProvider service, Type callerType)
        {
            return (IConfiguration)service.GetService(typeof(IConfiguration<>).MakeGenericType(callerType));
        }
        public static IConfiguration GetConfiguration<TCallerType>(this IServiceProvider service)
        {
            return (IConfiguration)service.GetService(typeof(IConfiguration<>).MakeGenericType(typeof(TCallerType)));
        }


        public static TValue GetValue<TValue>(this IConfiguration configuration, [CallerMemberName] string name = null, params object[] index)
        {
            _Binder binder = configuration as _Binder;
            if (binder == null)
                return ConfigurationBinder.GetValue(configuration, name, default(TValue));
            return binder.OnGetValue<TValue>(configuration, name, index);
        }

        public abstract class Provider : ConfigurationProvider
        {
            public abstract void OnInit(IServiceProvider service);
            public abstract bool OnGetValue<T>(string key, out T value, params object[] index);
        }

        private abstract class _Binder
        {
            private class _BinderMember
            {
                public AppSettingAttribute src;
                public MemberInfo Member;
                public DefaultValueAttribute DefaultValue;

                private Dictionary<Type, object> _defaultValues = new Dictionary<Type, object>();

                public TValue GetDefaultValue<TValue>()
                {
                    if (DefaultValue != null)
                    {
                        try
                        {
                            lock (_defaultValues)
                            {
                                if (_defaultValues.TryGetValue(typeof(TValue), out object tmp))
                                    return (TValue)tmp;
                                TValue result = DefaultValue.GetValue<TValue>();
                                _defaultValues[typeof(TValue)] = result;
                                return result;
                            }
                        }
                        catch { }
                    }
                    return default(TValue);
                }
            }

            protected IServiceProvider _service;
            protected IConfiguration _configuration;
            private _BinderMember[] _members;
            private Provider _provider;

            public _Binder(IServiceProvider service, IConfiguration configuration)
            {
                _service = service;
                _configuration = configuration;

                #region init
                if (configuration is IConfigurationRoot configRoot)
                {
                    foreach (var provider in configRoot.Providers)
                    {
                        if (provider is Provider obj)
                        {
                            _provider = obj;
                            obj.OnInit(service);
                        }
                    }
                }
                #endregion

                #region find members

                var members = new List<_BinderMember>();
                foreach (MemberInfo m in this.CallerType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    var attr = m.GetCustomAttribute<AppSettingAttribute>();
                    if (attr != null)
                    {
                        members.Add(new _BinderMember()
                        {
                            src = attr,
                            Member = m,
                            DefaultValue = m.GetCustomAttribute<DefaultValueAttribute>()
                        });
                    }
                }
                this._members = members.ToArray();

                #endregion
            }

            protected abstract Type CallerType { get; }

            public TValue OnGetValue<TValue>(IConfiguration configuration, string name, params object[] index)
            {
                if (configuration == null)
                    return default;

                if (name == null)
                    return default;

                for (int i = 0; i < _members.Length; i++)
                {
                    _BinderMember item = _members[i];
                    if (item.Member.Name == name)
                    {
                        string _key = item.src.Key ?? item.Member.Name;
                        TValue defaultValue = item.GetDefaultValue<TValue>();

                        if (_provider != null && _provider.OnGetValue<TValue>(_key, out var _value, index))
                            return _value;

                        return _configuration.GetValue<TValue>(_key, defaultValue);
                    }
                }
                return default;
            }
        }

        [DebuggerStepThrough]
        private class _Binder<TCallerType> : _Binder, IConfiguration<TCallerType>
        {
            public _Binder(IServiceProvider service, IConfiguration configuration)
                : base(service, configuration)
            {
            }

            protected override Type CallerType => typeof(TCallerType);

            string IConfiguration.this[string key]
            {
                get => _configuration[key];
                set => _configuration[key] = value;
            }

            IConfigurationSection IConfiguration.GetSection(string key) => _configuration.GetSection(key);

            IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _configuration.GetChildren();

            IChangeToken IConfiguration.GetReloadToken() => _configuration.GetReloadToken();
        }
    }
}