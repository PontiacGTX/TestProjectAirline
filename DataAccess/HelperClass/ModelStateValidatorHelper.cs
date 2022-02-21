using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass
{
    public static class ModelStateValidatorHelper
    {
        public static string GetModelErrors(this ModelStateDictionary ModelState)
        {
            string invalidFields = " the following fields are invalid: ";
            foreach (var item in ModelState.Keys)
            {
                if (ModelState[item].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                    invalidFields = string.Concat(invalidFields, $"{item} ({ModelState[item].Errors.FirstOrDefault().ErrorMessage}) ", " ");
            }
            return invalidFields;
        }
    }
}
