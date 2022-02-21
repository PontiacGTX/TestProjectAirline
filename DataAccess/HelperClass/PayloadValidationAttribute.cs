using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PayloadValidationAttribute: ValidationAttribute
    {
        public  int[] SkipValues { get; set; }
        public PayloadValidationAttribute(string Error)
        {
            ErrorMessage = Error;
        }
        public PayloadValidationAttribute()
        {
            ErrorMessage = $"Only the following values are allowed: ";
        }
        public override bool IsValid(object value)
        {
            if (value is not int or long or uint or ulong or byte or ushort or short)
                return false;

            List<int> payloadSizes = Enum.GetValues(typeof(PayloadSizeEnum)).Cast<PayloadSizeEnum>().Select(x=>(int)x).ToList();
            if (SkipValues != null)
            {
                foreach (var item in SkipValues)
                {
                    payloadSizes.Remove(item);
                }
            }
            bool isValid = payloadSizes.Any(x => x == (int)value);

            if (!isValid)
                ErrorMessage = string.Join(ErrorMessage, " ", string.Join("," , payloadSizes));

            return isValid;
        }
      
    }
}
