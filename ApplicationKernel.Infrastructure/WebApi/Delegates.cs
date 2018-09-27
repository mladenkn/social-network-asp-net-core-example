using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationKernel.Infrastructure.WebApi
{
    public delegate Task<IActionResult> HandleApiRequest(IRequest request);
}