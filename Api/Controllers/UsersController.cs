using Api.Common;
using Application.Users.User;
using Domain.Persistence.User;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAllUsers([FromServices] IUserUnitOfWork unitOfWork)
    {
        var requestHandler = new GetAllUsersRequestHandler(unitOfWork);
        var result = await requestHandler.ProcessAuthorizedRequestAsync(new GetAllUsersRequest());
        return result.ToActionResult(this);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserById([FromServices] IUserUnitOfWork unitOfWork, [FromRoute] int id)
    {
        var requestHandler = new GetUserByIdRequestHandler(unitOfWork);
        var result = await requestHandler.ProcessAuthorizedRequestAsync(new GetUserByIdRequest{Id = id});
        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request, [FromServices] IUserUnitOfWork unitOfWork)
    {
        var requestHandler = new CreateUserRequestHandler(unitOfWork);
        var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
        return result.ToActionResult(this);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserRequest request, [FromServices] IUserUnitOfWork unitOfWork,
        [FromRoute] int id)
    {
        var requestHandler = new UpdateUserRequestHandler(unitOfWork);
        request.Id = id;
        var result = await requestHandler.ProcessAuthorizedRequestAsync(request);
        return result.ToActionResult(this);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser([FromServices] IUserUnitOfWork unitOfWork, [FromRoute] int id)
    {
        var requestHandler = new DeleteUserRequestHandler(unitOfWork);
        var result = await requestHandler.ProcessAuthorizedRequestAsync(new DeleteUserRequest { Id = id });
        return result.ToActionResult(this);
    }

    [HttpPost("activate/{id}")]
    public async Task<ActionResult> ActivateUser([FromServices] IUserUnitOfWork unitOfWork, [FromRoute] int id)
    {
        var requestHandlet = new ActivateUserRequestHandler(unitOfWork);
        var result = await requestHandlet.ProcessAuthorizedRequestAsync(new ActivateUserRequest{Id = id});
        return result.ToActionResult(this);
    }

    [HttpPost("deactivate/{id}")]
    public async Task<ActionResult> DeactivateUser([FromServices] IUserUnitOfWork unitOfWork, [FromRoute] int id)
    {
        var requestHandler = new DeactivateUserRequestHandler(unitOfWork);
        var result = await requestHandler.ProcessAuthorizedRequestAsync(new DeactivateUserRequest { Id = id });
        return result.ToActionResult(this);
    }

    [HttpPost("import-external")]
    public async Task<ActionResult> ImportExternalUsers([FromServices] IUserUnitOfWork unitOfWork, [FromServices]
        IRetrieveExternalUsers
            externalUserService, [FromServices] ICacheServices cacheService)
    {
        var requestHandler = new GetExternalUsersRequestHandler(unitOfWork, externalUserService, cacheService);
        var result = await  requestHandler.ProcessAuthorizedRequestAsync(new GetExternalUsersRequest());
        return result.ToActionResult(this);
    }
    
}