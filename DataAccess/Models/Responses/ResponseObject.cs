﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Responses
{
    public class ResponseObject
    {
        public virtual int Code { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public object  Data { get; set; }
    }
}
