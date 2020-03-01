using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Api.Contracts
{
    public interface ICacheStore
    {
        Task SetObjectAsync<T>(IDistributedCache cache, string key, T value, long validityInSeconds);
        Task<T> GetObjectAsync<T>(IDistributedCache cache, string key);
        Task<bool> ExistObjectAsync<T>(IDistributedCache cache, string key);
    }
}
