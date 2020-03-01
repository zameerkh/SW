using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SW.Api.Contracts;
using SW.Api.Models;

namespace SW.Api.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly int CacheTimeoutInSeconds = 3600;
        public IPersonSource PersonSource { get; set; }

        public ICacheStore CacheStore { get; set; }
        public IDistributedCache Cache { get; set; }


        public PersonsController(IPersonSource personSource,
                                ICacheStore cacheStore,
                                IDistributedCache cache)
        {
            PersonSource = personSource;
            CacheStore = cacheStore;
            Cache = cache;
        }


        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Person>> GetById(int id)
        {
            var cacheKey = $"GetById_{id}";
            var cachedValue = await CacheStore.GetObjectAsync<Person>(Cache, cacheKey);

            if (cachedValue == null)
            {
                var person = await PersonSource.GetById(id);

                if (person == null)
                {
                    return NotFound();
                }
  
                await CacheStore.SetObjectAsync<Person>(Cache, cacheKey, person, CacheTimeoutInSeconds);
                return Ok(person);
            }
 
            return Ok(cachedValue);
        }


        [Route("[action]/{age}")]
        [HttpGet("{age}")]
        public async Task<ActionResult<IList<Person>>> GetByAge(int age)
        {
            var cacheKey = $"GetByAge{age}";
            var cachedValue = await CacheStore.GetObjectAsync<List<Person>>(Cache, cacheKey);

            if (cachedValue == null)
            {
                var persons = await PersonSource.GetByAge(age);

                if (persons == null)
                {
                    return NotFound();
                }

                await CacheStore.SetObjectAsync<List<Person>>(Cache, cacheKey, persons.ToList(), CacheTimeoutInSeconds);
                return Ok(persons.ToList());
            }

            return Ok(cachedValue);
        }

        [HttpGet()]
        [Route("[action]")]
        public async Task<ActionResult<IList<GenderStatistics>>> GenderStatistics(int age)
        {
            var cacheKey = $"GenderStatistics";
            var cachedValue = await CacheStore.GetObjectAsync<List<GenderStatistics>>(Cache, cacheKey);

            if (cachedValue == null)
            {
                var stats = await PersonSource.GenderStatistics();

                if (stats == null)
                {
                    return NotFound();
                }

                await CacheStore.SetObjectAsync<List<GenderStatistics>>(Cache, cacheKey, stats.ToList(), CacheTimeoutInSeconds);

                return Ok(stats);
            }

            return Ok(cachedValue);
        }
    }
}