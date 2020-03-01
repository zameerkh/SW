using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SW.Api
{
    public class InternalServerErrorCustomException : BaseCustomException
    {
        public InternalServerErrorCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.InternalServerError)
        {
        }
    }
}
