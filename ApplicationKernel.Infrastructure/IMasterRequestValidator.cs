using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation.Results;

namespace ApplicationKernel.Infrastructure
{
    public interface IMasterRequestValidator
    {
        ValidationResult Validate(IRequest request);
    }
}
