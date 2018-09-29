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
        private readonly IValidatorProvider _requestValidatorProvider;
        private readonly MapToActionResult _mapToActionResult;

        public ApiRequestHandler(
            IMediator mediator, 
            IValidatorProvider requestValidatorProvider, 
            MapToActionResult mapToActionResult)
        {
            _mediator = mediator;
            _requestValidatorProvider = requestValidatorProvider;
            _mapToActionResult = mapToActionResult;
        }

        public async Task<IActionResult> Handle(IRequest request)
        {
            var validator = _requestValidatorProvider.ValidatorOf(request.GetType());

            if (validator != null)
            {
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult(new
                    {
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });
                }
            }
            var response = await _mediator.Send(request);
            return _mapToActionResult(response);
        }
    }
}
