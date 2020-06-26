using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GLT
{
    public static class SqlMapperTypeHandleing
    {
        static object sync = new object();
        static bool isInit = false;

        public static void Init()
        {
            if (isInit) return;
            lock (sync)
            {
                if (isInit) return;
                SqlMapper.AddTypeHandler(typeof(SqlTimeStamp), new SqlTimeStampHandler());
                isInit = true;
            }
        }

        class SqlTimeStampHandler : SqlMapper.ITypeHandler
        {
            object SqlMapper.ITypeHandler.Parse(Type destinationType, object value)
            {
                if (destinationType == typeof(SqlTimeStamp) && value is byte[] _value)
                    return (SqlTimeStamp)_value;
                return null;
            }

            void SqlMapper.ITypeHandler.SetValue(IDbDataParameter parameter, object value)
            {
                throw new NotImplementedException();
            }
        }
}
}
