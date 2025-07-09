using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Utilities.Results.Interfaces;

namespace Domain.Utilities.Results.Implementations
{
    public class Result : IResult
    {
        public string Message {get;}

        public bool Success {get;}

        public Result(bool success, string message):this(success)
        {
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
        }
    }
}