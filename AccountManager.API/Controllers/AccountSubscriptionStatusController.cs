using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountSubscriptionStatusController(IAccountSubscriptionStatusService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AccountSubscriptionStatus>>> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountSubscriptionStatus>> GetById(int id)
    {
        var status = await service.GetByIdAsync(id);
        if (status == null) return NotFound();
        return Ok(status);
    }

    [HttpPost]
    public async Task<ActionResult<AccountSubscriptionStatus>> Create([FromBody] AccountSubscriptionStatus status)
    {
        var created = await service.CreateAsync(status);
        return CreatedAtAction(nameof(GetById), new { id = created.SubscriptionStatusId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AccountSubscriptionStatus>> Update(int id,
        [FromBody] AccountSubscriptionStatus status)
    {
        if (id != status.SubscriptionStatusId) return BadRequest();
        return Ok(await service.UpdateAsync(status));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}