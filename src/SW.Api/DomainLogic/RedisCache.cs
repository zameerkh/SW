using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SW.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Api
{
    public class RedisCache : ICacheStore
    {
        // save 
        public async Task SetObjectAsync<T>(IDistributedCache cache, string key, T value, long validityInSeconds)
        {
            try
            {
                await cache.SetStringAsync(key, JsonConvert.SerializeObject(value),
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(validityInSeconds)
                    });
            }
            catch (Exception e)
            {
                throw new InternalServerErrorCustomException("I did bad!", e.StackTrace);
            }


        }

        // get
        public async Task<T> GetObjectAsync<T>(IDistributedCache cache, string key)
        {
            try
            {
                var value = await cache.GetStringAsync(key);
                return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorCustomException("I did bad!", e.StackTrace);
            }

        }

        // verify if an object exists
        public async Task<bool> ExistObjectAsync<T>(IDistributedCache cache, string key)
        {
            try
            {
                var value = await cache.GetStringAsync(key);
                return value != null;
            }
            catch (Exception e)
            {
                throw new InternalServerErrorCustomException("I did bad!", e.StackTrace);
            }

        }
    }
}
