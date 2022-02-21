using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public enum  ErrorStringEnum
    {
        FailedRequest =-1,
        NotFound =0,
        InternalServerError=1,
        BadRequestError=2,

    }
}
