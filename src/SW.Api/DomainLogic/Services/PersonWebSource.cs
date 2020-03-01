using Newtonsoft.Json;
using SW.Api.Contracts;
using SW.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SW.Api.Services
{
    public class PersonWebSource : IPersonSource
    {
        public List<Person> Persons { get; set; }
        public PersonWebSource()
        {
            Persons = new List<Person>();

        }

        public async Task Init()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://f43qgubfhf.execute-api.ap-southeast-2.amazonaws.com");
                httpClient.Timeout = new TimeSpan(0, 0, 30);
                httpClient.DefaultRequestHeaders.Clear();

                var response = await httpClient.GetAsync("sampletest");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    Persons = JsonConvert.DeserializeObject<List<Person>>(content);
                }
                else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                {
                    var serializer = new XmlSerializer(typeof(List<Person>));
                    Persons = (List<Person>)serializer.Deserialize(new StringReader(content));
                }
            }
        }
        public async Task<IList<GenderStatistics>> GenderStatistics()
        {
            if(!Persons.Any())
            {
                await Init();
            }

            var query = from person in Persons
                        group person by person.age into ageGroup
                        orderby ageGroup.Key
                        select ageGroup;
            var agrGroupArray = query.ToList();

            var genderStats = new List<GenderStatistics>();

            foreach (var group in agrGroupArray)
            {
                var stat = new GenderStatistics();
                stat.age = group.Key;
                foreach (var person in group)
                {
                    if(person.gender == "M")
                    {
                        stat.male++;
                    }
                    else if (person.gender == "F")
                    {
                        stat.female++;
                    }
                    else
                    {
                        stat.others++;
                    }
                }

                genderStats.Add(stat);


            }
            return genderStats;
        }

        public async Task<IList<Person>> GetByAge(int age)
        {
            if (!Persons.Any())
            {
                await Init();
            }

            return Persons.Where(p => p.age == age).ToList();

        }

        public async Task<Person> GetById(int id)
        {
            if (!Persons.Any())
            {
                await Init();
            }
            return Persons.FirstOrDefault(p => p.id == id);

        }
    }
}
