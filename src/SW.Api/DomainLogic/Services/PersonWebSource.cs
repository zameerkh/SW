using SW.Api.Contracts;
using SW.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SW.Api.Services
{
    public class PersonWebSource : IPersonSource
    {
        public List<Person> Persons { get; set; }

        public IGetPersons PersonSource { get; set; }
        public PersonWebSource(IGetPersons personSource)
        {
            Persons = new List<Person>();
            PersonSource = personSource;
        }

        public async Task Init()
        {
            var persons = await PersonSource.GetPersons();
            Persons = persons.ToList();
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
