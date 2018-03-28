using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SocialNetwork
{
    public class ExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
        }
    }
}
