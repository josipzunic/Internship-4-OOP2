using Api.Common;
using Application.Companies;
using Domain.Persistence.Companies;
using Domain.Persistence.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAllCompanies(
        [FromServices] ICompanyUnitOfWork unitOfWork, [FromServices]  IUserUnitOfWork userUnitOfWork,
        [FromQuery] string username, [FromQuery] string password)
    {
        var handler = new GetAllCompaniesRequestHandler(unitOfWork, userUnitOfWork);

        var result = await handler.ProcessAuthorizedRequestAsync(
            new GetAllCompaniesRequest
            {
                Username = username,
                Password = password
            });

        return result.ToActionResult(this);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCompanyById(
        [FromServices] ICompanyUnitOfWork unitOfWork,
        [FromRoute] int id, [FromServices] IUserUnitOfWork userUnitOfWork,
        [FromQuery] string username, [FromQuery] string password)
    {
        var handler = new GetCompanyByIdRequestHandler(unitOfWork, userUnitOfWork);

        var result = await handler.ProcessAuthorizedRequestAsync(
            new GetCompanyByIdRequest
            {
                Id = id,
                Username = username,
                Password = password
            });

        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCompany(
        [FromBody] CreateCompanyRequest request,
        [FromServices] ICompanyUnitOfWork unitOfWork, [FromServices] IUserUnitOfWork userUnitOfWork,
        [FromQuery] string username, [FromQuery] string password)
    {
        request.Username = username;
        request.Password = password;

        var handler = new CreateCompanyRequestHandler(unitOfWork,  userUnitOfWork);

        var result = await handler.ProcessAuthorizedRequestAsync(request);
        return result.ToActionResult(this);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCompany(
        [FromBody] UpdateCompanyRequest request,
        [FromServices] ICompanyUnitOfWork unitOfWork, [FromServices] IUserUnitOfWork userUnitOfWork,
        [FromRoute] int id, [FromQuery] string username, [FromQuery] string password)
    {
        request.Id = id;
        request.Username = username;
        request.Password = password;

        var handler = new UpdateCompanyRequestHandler(unitOfWork,  userUnitOfWork);

        var result = await handler.ProcessAuthorizedRequestAsync(request);
        return result.ToActionResult(this);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompany(
        [FromServices] ICompanyUnitOfWork unitOfWork, [FromServices]  IUserUnitOfWork userUnitOfWork,
        [FromRoute] int id, [FromQuery] string username, [FromQuery] string password)
    {
        var handler = new DeleteCompanyRequestHandler(unitOfWork,  userUnitOfWork);

        var result = await handler.ProcessAuthorizedRequestAsync(
            new DeleteCompanyRequest
            {
                Id = id,
                Username = username,
                Password = password
            });

        return result.ToActionResult(this);
    }
}
