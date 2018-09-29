using System.Linq;
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
                var payloadProperty = response.GetType().GetProperties().FirstOrDefault(p => p.Name == "Payload");

                if (payloadProperty != null)
                {
                    var payload = payloadProperty.GetValue(response);
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
