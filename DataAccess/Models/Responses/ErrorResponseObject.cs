using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Responses
{
   public  class ErrorResponseObject:ResponseObject
    {
        public override int Code { get; set; } = 500;
    }
}
