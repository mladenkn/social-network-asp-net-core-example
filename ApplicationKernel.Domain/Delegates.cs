using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation.Results;

namespace ApplicationKernel.Domain
{
    public delegate ValidationResult ValidateRequest(IRequest request);
}
