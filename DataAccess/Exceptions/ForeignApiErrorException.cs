using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Exceptions
{
    public class ForeignApiErrorException:Exception
    {
        public int ApiError { get; set; }
        public ForeignApiErrorException(string message):base(message)
        {

        }
        public ForeignApiErrorException(string message,int apiErrorCode) : base(message)
        {
            ApiError = apiErrorCode;
        }
        public ForeignApiErrorException(string message, Exception ex):base(message, ex)
        {

        }
    }
}
