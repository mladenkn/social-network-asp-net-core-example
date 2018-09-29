using System;
using FluentValidation;

namespace ApplicationKernel.Infrastructure
{
    public interface IValidatorProvider
    {
        IValidator ValidatorOf(Type type);
    }
}
