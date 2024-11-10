using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class CountriesExternalApiException : Exception
    {
        public CountriesExternalApiException(string message) : base(message)
        {
        }

        public CountriesExternalApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
