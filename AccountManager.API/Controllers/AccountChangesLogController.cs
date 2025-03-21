using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountChangesLogController(IAccountChangesLogService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AccountChangesLog>>> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<List<AccountChangesLog>>> GetByAccount(int accountId)
    {
        return Ok(await service.GetByAccountAsync(accountId));
    }

    [HttpPost]
    public async Task<ActionResult<AccountChangesLog>> Create([FromBody] AccountChangesLog log)
    {
        var created = await service.CreateAsync(log);
        return CreatedAtAction(nameof(GetAll), null, created);
    }

    [HttpDelete("{logId}")]
    public async Task<IActionResult> Delete(int logId)
    {
        await service.DeleteAsync(logId);
        return NoContent();
    }
}