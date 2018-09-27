using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IRequest = ApplicationKernel.Domain.MediatorSystem.IRequest;

namespace ApplicationKernel.Infrastructure.WebApi
{
    public class ApiRequestHandler
    {
        private readonly IMediator _mediator;
        private readonly IMasterRequestValidator _requestValidator;
        private readonly MapToActionResult _mapToActionResult;

        public ApiRequestHandler(
            IMediator mediator, 
            IMasterRequestValidator requestValidator, 
            MapToActionResult mapToActionResult)
        {
            _mediator = mediator;
            _requestValidator = requestValidator;
            _mapToActionResult = mapToActionResult;
        }

        public async Task<IActionResult> Handle(IRequest request)
        {
            var validationResult = _requestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(new
                {
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }
            var response = await _mediator.Send(request);
            return _mapToActionResult(response);
        }
    }
}
