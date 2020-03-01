using SW.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Api.Contracts
{
    public interface IGetPersons
    {
        Task<IList<Person>> GetPersons();
    }
}
