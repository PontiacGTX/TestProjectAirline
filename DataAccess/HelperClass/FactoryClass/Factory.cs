using DataAccess.Models;
using DataAccess.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.HelperClass.FactoryClass
{
    public static class Factory
    {
        public static string GetError(string methodName, string error,string parameters=null)
        {
            return string.Concat("An Error Ocurred in ", methodName,string.IsNullOrEmpty(parameters) ? parameters:$" with parameters {parameters} ", "due to ", error);
        }
        public static ResponseObject GetResponse<T>(object data,int responseCode =200, string message="Success") 
        {
            Type typeResponse = typeof(T);
            if (typeResponse == typeof(ResponseObject))
                return new ResponseObject { Code = responseCode, Message = message, Data = data, Success = true };
            else if(typeResponse == typeof(ErrorResponseObject))
                return new ErrorResponseObject { Code =  responseCode is not 200? responseCode:500, Message = message, Data = data, Success = false };

            return null;
        }
        public static string GetStringError(ErrorStringEnum error)=>
        error switch
        {
            ErrorStringEnum.FailedRequest =>"An error occurred while doing a request",
            ErrorStringEnum.NotFound =>"Couldn't find any entry with the requested value",
            ErrorStringEnum.InternalServerError =>"There was an internal server error",
            ErrorStringEnum.BadRequestError=>"The model sent was invalid due to one or more validations errors",
            _ => "An unkonwn/unexpected error happened"
        };


    }
}
