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
    private readonly CreateJobApplicationHandler createHandler;
    private readonly GetAllJobApplicationsHandler getAllHandler;
    private readonly GetJobApplicationHandler getHandler;
    private readonly UpdateJobApplicationHandler updateHandler;
    private readonly DeleteJobApplicationHandler deleteHandler;

    public JobApplicationsController(
        CreateJobApplicationHandler createHandler,
        GetAllJobApplicationsHandler getAllHandler,
        GetJobApplicationHandler getHandler,
        UpdateJobApplicationHandler updateHandler,
        DeleteJobApplicationHandler deleteHandler)
    {
        this.createHandler = createHandler;
        this.getAllHandler = getAllHandler;
        this.getHandler = getHandler;
        this.updateHandler = updateHandler;
        this.deleteHandler = deleteHandler;
    }

    private Guid GetCurrentUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllJobApplicationsQuery(GetCurrentUserId());
        var result = await getAllHandler.HandleAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetJobApplicationQuery(id, GetCurrentUserId());
            var result = await getHandler.HandleAsync(query, cancellationToken);
            return Ok(result);
        }
        catch (Application.Common.Exceptions.NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateJobApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var commandWithUser = command with { UserId = GetCurrentUserId() };
        var result = await createHandler.HandleAsync(commandWithUser, cancellationToken);
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

        try
        {
            var commandWithUser = command with { UserId = GetCurrentUserId() };
            var result = await updateHandler.HandleAsync(commandWithUser, cancellationToken);
            return Ok(result);
        }
        catch (Application.Common.Exceptions.NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteJobApplicationCommand(id, GetCurrentUserId());
            await deleteHandler.HandleAsync(command, cancellationToken);
            return NoContent();
        }
        catch (Application.Common.Exceptions.NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}