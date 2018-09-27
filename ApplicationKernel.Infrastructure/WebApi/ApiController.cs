using System.Threading.Tasks;
using ApplicationKernel.Domain;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationKernel.Infrastructure.WebApi
{
    public class ApiController : ControllerBase
    {
        private readonly HandleApiRequest _handleApiRequest;

        public ApiController(HandleApiRequest handleApiRequest)
        {
            _handleApiRequest = handleApiRequest;
        }

        protected Task<IActionResult> HandleRequest(IRequest request) => _handleApiRequest(request);
    }
}
