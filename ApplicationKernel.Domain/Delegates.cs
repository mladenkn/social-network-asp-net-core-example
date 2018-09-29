using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation.Results;

namespace ApplicationKernel.Domain
{
    public delegate ValidationResult ValidateRequest(IRequest request);
}
