using DataAccess.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Requests
{
    public class JourneyRequestModel
    {
        [StringValidation(Pattern ="[a-zñáéíóúA-ZÑÁÉÍÓÚ]")]
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}
