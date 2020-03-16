using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dapper.Json
{
    internal static class SqlTypeMapping
    {
        internal static string MatchType(PropertyInfo info)
        {
            TypeCode code = Type.GetTypeCode(info.PropertyType);
            switch(code)
            {
                case (TypeCode.Boolean):
                    return "bit";
                case (TypeCode.String):
                    return "varchar(50)";
                case (TypeCode.Int32):
                    return "int";
                case (TypeCode.Decimal):
                    return "numeric(12,2)";
                case (TypeCode.DateTime):
                    return "datetime";
                default:
                    return "varchar(50)";
            }
        }
    }
}
