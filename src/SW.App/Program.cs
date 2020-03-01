using Newtonsoft.Json;
using SW.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SW.App
{
    class Program
    {
        private static TimeSpan TimeOut = new TimeSpan(0,0,30); 
        static void Main(string[] args)
        {
            TestDetails().GetAwaiter().GetResult();
        }

        private static async Task TestDetails()
        {
            await TestGetPersonDetailsById();
            await TestGetPersonsDetailsByAge();
            await TestGenderStatistics();
            Console.ReadKey();


        }

        private static async Task TestGetPersonDetailsById()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:9000");
                httpClient.Timeout = TimeOut;
                httpClient.DefaultRequestHeaders.Clear();

                var response = await httpClient.GetAsync("api/persons/GetById/53");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var person = JsonConvert.DeserializeObject<Person>(content);
                    Console.WriteLine($"Name : {person.First}  {person.last} ");
                }
                else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                {
                    var serializer = new XmlSerializer(typeof(Person));
                    var person = (Person)serializer.Deserialize(new StringReader(content));
                }
            }
        }


        private static async Task TestGetPersonsDetailsByAge()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:9000");
                httpClient.Timeout = TimeOut;
                httpClient.DefaultRequestHeaders.Clear();

                var response = await httpClient.GetAsync("api/persons/GetByAge/66");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var persons = JsonConvert.DeserializeObject<List<Person>>(content);

                    PrintPerson(persons);

                }
                else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                {
                    var serializer = new XmlSerializer(typeof(Person));
                    var persons = (List<Person>)serializer.Deserialize(new StringReader(content));

                    PrintPerson(persons);
                }
            }

        }

        private static void PrintPerson(List<Person> persons)
        {
            foreach (var person in persons)
            {
                Console.WriteLine($"Name : {person.First}  {person.last}, Age : {person.age} , Gender: {person.gender} ");
            }
        }

        private static async Task TestGenderStatistics()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:9000");
                httpClient.Timeout = TimeOut;
                httpClient.DefaultRequestHeaders.Clear();

                var response = await httpClient.GetAsync("api/persons/GenderStatistics");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var stats = JsonConvert.DeserializeObject<List<GenderStatistics>>(content);
                    PrintStats(stats);

                }
                else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                {
                    var serializer = new XmlSerializer(typeof(Person));
                    var stats = (List<GenderStatistics>)serializer.Deserialize(new StringReader(content));
                    PrintStats(stats);
                }
            }
        }

        private static void PrintStats(List<GenderStatistics> stats)
        {
            foreach (var stat in stats)
            {
                Console.WriteLine($"Age : {stat.age}  , Male : {stat.male} , Female: {stat.female} , Others : {stat.others}");
            }
        }
    }
    
}
