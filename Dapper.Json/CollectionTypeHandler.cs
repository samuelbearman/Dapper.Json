using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using static Dapper.SqlMapper;

namespace Dapper.Json
{
    public abstract class CollectionTypeHandler<T> : ITypeHandler
    {
        public abstract void SetValue(IDbDataParameter parameter, IEnumerable<T> value);
        public abstract IEnumerable<T> Parse(object value);

        void ITypeHandler.SetValue(IDbDataParameter parameter, object value)
        {
            if (value is DBNull)
            {
                parameter.Value = value;
            }
            else
            {
                SetValue(parameter, (IEnumerable<T>)value);
            }
        }

        object ITypeHandler.Parse(Type destinationType, object value)
        {
            return Parse(value);
        }
    }
}