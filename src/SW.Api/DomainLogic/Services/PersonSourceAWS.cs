using Newtonsoft.Json;
using SW.Api.Contracts;
using SW.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SW.Api.Services
{
    public class PersonSourceAWS : IGetPersons
    {
        public async Task<IList<Person>> GetPersons()
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
                    return JsonConvert.DeserializeObject<List<Person>>(content);
                }
                else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                {
                    var serializer = new XmlSerializer(typeof(List<Person>));
                    return  (List<Person>)serializer.Deserialize(new StringReader(content));
                }
            }

            return null;
        }
    }
}
