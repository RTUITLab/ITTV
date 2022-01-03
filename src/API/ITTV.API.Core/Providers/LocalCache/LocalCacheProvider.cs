using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ITTV.API.Core.Providers.LocalCache
{
    public static class LocalCacheProvider
    {
        private const string BaseCachePath = "Cache";
        public static FileInfo GetCacheInfo(string key)
            => new($"{BaseCachePath}/{key}.json");

        public static async Task<T> GetAsync<T>(string key, Func<Task<T>> dataProvider, TimeSpan expire = default, bool useExists = false)
        {
            var fileInfo = GetCacheInfo(key);
            if (useExists || fileInfo.Exists && DateTime.Now - fileInfo.LastWriteTimeUtc >= expire)
            {
                var cache = await GetAsync<T>(key, expire);
                return cache;
            }
            
            var data = await dataProvider();
            await PutAsync(key, data);
            return data;
        }

        public static async Task<T> GetAsync<T>(string key, TimeSpan expire = default)
        {
            var file = GetCacheInfo(key);
            
            if (file is not {Exists: true} || expire != default && DateTime.Now - file.LastWriteTimeUtc <= expire) 
                return default;
            
            var fileData = await File.ReadAllTextAsync(file.FullName);
            return JsonConvert.DeserializeObject<T>(fileData);

        }
        public static async Task PutAsync<T>(string key, T value)
        {
            var file = GetCacheInfo(key);
            
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            await File.WriteAllTextAsync(file.FullName,JsonConvert.SerializeObject(value), Encoding.UTF8);
        }
    }
}