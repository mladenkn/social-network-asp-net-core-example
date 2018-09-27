using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationKernel.Infrastructure
{
    public class MasterRequestValidator : IMasterRequestValidator
    {
        private readonly IServiceProvider _serviceProvider;

        public MasterRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ValidationResult Validate(IRequest request)
        {
            var validators = _serviceProvider.GetServices<IValidator>();
            var thisRequestValidator = validators.Single(it => it.CanValidateInstancesOfType(request.GetType()));
            return thisRequestValidator.Validate(request);
        }
    }
}
