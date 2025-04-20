using IdentityService.API.Requests;
using IdentityService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class IdentityController : Controller
    {
        #region private
        private readonly IMediator _mediator;
        #endregion

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> UserRegistration(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var command = new UserRegistrationCommand(request.Username, request.Email, request.Password);

            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UserAuthorization(UserAuthorizationRequest request, CancellationToken cancellationToken)
        {
            var command = new UserAuthorizationCommand(request.Username, request.Password);

            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}
