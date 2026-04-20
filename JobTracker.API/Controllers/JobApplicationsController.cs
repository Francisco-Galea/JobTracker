using System.Security.Claims;
using JobTracker.Application.UseCases.CreateJobApplication;
using JobTracker.Application.UseCases.DeleteJobApplication;
using JobTracker.Application.UseCases.GetAllJobApplications;
using JobTracker.Application.UseCases.GetJobApplication;
using JobTracker.Application.UseCases.UpdateJobApplication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    private readonly CreateJobApplicationHandler _createHandler;
    private readonly GetAllJobApplicationsHandler _getAllHandler;
    private readonly GetJobApplicationHandler _getHandler;
    private readonly UpdateJobApplicationHandler _updateHandler;
    private readonly DeleteJobApplicationHandler _deleteHandler;

    public JobApplicationsController(
        CreateJobApplicationHandler createHandler,
        GetAllJobApplicationsHandler getAllHandler,
        GetJobApplicationHandler getHandler,
        UpdateJobApplicationHandler updateHandler,
        DeleteJobApplicationHandler deleteHandler)
    {
        _createHandler = createHandler;
        _getAllHandler = getAllHandler;
        _getHandler = getHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    private Guid GetCurrentUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllJobApplicationsQuery(GetCurrentUserId());
        var result = await _getAllHandler.HandleAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetJobApplicationQuery(id, GetCurrentUserId());
        var result = await _getHandler.HandleAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateJobApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var commandWithUser = command with { UserId = GetCurrentUserId() };
        var result = await _createHandler.HandleAsync(commandWithUser, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateJobApplicationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(new { message = "El ID de la URL no coincide con el del body." });

        var commandWithUser = command with { UserId = GetCurrentUserId() };
        var result = await _updateHandler.HandleAsync(commandWithUser, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteJobApplicationCommand(id, GetCurrentUserId());
        await _deleteHandler.HandleAsync(command, cancellationToken);
        return NoContent();
    }
}