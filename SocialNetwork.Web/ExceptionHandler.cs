using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SocialNetwork.Web
{
    public class ExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
        }
    }
}
