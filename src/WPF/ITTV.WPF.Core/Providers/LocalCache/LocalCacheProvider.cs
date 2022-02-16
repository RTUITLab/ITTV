using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ITTV.WPF.Core.Helpers;
using Newtonsoft.Json;

namespace ITTV.WPF.Core.Providers.LocalCache
{
    public static class LocalCacheProvider
    { 
        public static FileInfo GetCacheFileInfo(string key)
            => new($"{PathHelper.GetDirectoryCachePath}/{key}.json");

        public static async Task<T> GetAsync<T>(string key, Func<Task<T>> dataProvider, TimeSpan expire = default, bool useExists = false)
        {
            var fileInfo = GetCacheFileInfo(key);
            if (useExists || fileInfo.Exists && DateTime.Now - fileInfo.LastWriteTime <= expire)
            {
                var cache = Get<T>(key);
                
                if (cache != null)
                    return cache;
            }
            
            var data = await dataProvider();
            Put(key, data);
            return data;
        }

        public static T Get<T>(string key)
        {
            var file = GetCacheFileInfo(key);
            
            var fileData =  File.ReadAllText(file.FullName);
            return JsonConvert.DeserializeObject<T>(fileData);
        }
        public static void Put<T>(string key, T value)
        {
            var file = GetCacheFileInfo(key);
            
            if (file.Directory is {Exists: false})
            {
                file.Directory.Create();
            }
            File.WriteAllText(file.FullName,JsonConvert.SerializeObject(value), Encoding.UTF8);
        }
    }
}