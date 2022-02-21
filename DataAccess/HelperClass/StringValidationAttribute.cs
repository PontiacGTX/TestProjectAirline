using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.HelperClass
{
    class StringValidationAttribute:ValidationAttribute
    {
        public string Pattern { get; set; }
        public StringValidationAttribute()
        {

        }
        public override bool IsValid(object value)
        {
            if (value is not string)
                return false;

            if (string.IsNullOrEmpty(value.ToString()))
                return false;

            Regex regEx = new Regex(Pattern);
            string field = value.ToString();
            
            return regEx.Match(field).Success;
        }
    }
}
