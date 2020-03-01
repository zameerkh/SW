using SW.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Api.Contracts
{
    public interface IPersonSource
    {
        Task<Person> GetById(int id);
        Task<IList<Person>> GetByAge(int age);
        Task<IList<GenderStatistics>> GenderStatistics();
    }
}
