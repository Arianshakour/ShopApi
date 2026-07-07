using Shop.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Text.Json;

namespace Shop.Application.Services.Implementation
{
    public class RedisCacheService : ICacheService
    {
        //Cache bayad to laye Application bashe chon Business bayad tashkhis bede aval az cache bekhoone
        //repository karesh database hast nabayad cache dargir konim
        //solid o naqz mikonim age bebarim to repository
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();//injoori vasl shodim be database Redis
        }
        //inja darim dar redis misazim key,value va time enqeza
        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            //aval json kardim
            //inja mirim dar redis zakhire mikonim
            //when: When.Always yani age ba on key vojod dasht Overwrite kon
            //masalan id 1 ba price 50 hast hala price shode 60 boro id 1 o price 60 zakhire kon dg
            _database.StringSet(key: key, value: json, expiry: expiry, when: When.Always);
        }
        //inja az redis mikhoonim
        public T? Get<T>(string key)
        {
            //inja kole dto ra mikhone ke json kardimesh
            var value = _database.StringGet(key);

            if (value.IsNullOrEmpty)
                return default;
            //inja az json be dto taqir mide va mide be ma
            return JsonSerializer.Deserialize<T>(value!);
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        //ma roye search haye list dg version midim ke majboor nabashim badaz update/delete/create berim kole
        // key haye marboot be list ra hazf konim az redis
        //chon hazinash balast
        public int GetVersion(string key)
        {
            var value = _database.StringGet(key);

            if (value.IsNullOrEmpty)
            {
                return 1;
            }

            return (int)value;
        }

        public long IncrementVersion(string key)
        {
            return _database.StringIncrement(key);
        }
    }

}
