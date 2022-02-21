
using DataAccess.HelperClass;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models.Requests
{
    public class PayloadRequestModel
    {
        [JsonPropertyName("payloadSize")]
        [FromQuery]
        [PayloadValidation(SkipValues =new int[] { 4})]
        public int PayloadSize { get; set; }
    }
}
