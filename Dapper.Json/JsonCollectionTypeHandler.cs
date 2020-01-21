using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace Dapper.Json
{
    public class JsonCollectionTypeHandler<T> : CollectionTypeHandler<T>
    {
        public override IEnumerable<T> Parse(object value)
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, IEnumerable<T> value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}