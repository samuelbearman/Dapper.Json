using System.Data;
using Newtonsoft.Json;

namespace Dapper.Json
{
    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override T Parse(object value)
        {
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}