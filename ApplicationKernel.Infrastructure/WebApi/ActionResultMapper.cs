using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationKernel.Infrastructure.WebApi
{
    public class ActionResultMapper
    {
        public IActionResult Map(Response response)
        {
            if (response.IsSuccess)
            {
                if (response.GetType().GetProperties().Any(p => p.Name == "Payload"))
                {
                    var payload = response.GetType().GetProperty("Payload").GetValue(response);
                    return new OkObjectResult(payload);
                }
                else
                    return new OkResult();
            }
            else
            {
                if (response.ErrorMessage == null)
                    return new BadRequestResult();
                else
                    return new BadRequestObjectResult(response.ErrorMessage);
            }
        }
    }

    public delegate IActionResult MapToActionResult(Response response);
}
