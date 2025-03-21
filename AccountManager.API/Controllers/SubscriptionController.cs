using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController(ISubscriptionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Subscription>>> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetById(int id)
    {
        var s = await service.GetByIdAsync(id);
        if (s == null) return NotFound();
        return Ok(s);
    }

    [HttpPost]
    public async Task<ActionResult<Subscription>> Create([FromBody] Subscription sub)
    {
        var created = await service.CreateAsync(sub);
        return CreatedAtAction(nameof(GetById), new { id = created.SubscriptionId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Subscription>> Update(int id, [FromBody] Subscription sub)
    {
        if (id != sub.SubscriptionId) return BadRequest();
        return Ok(await service.UpdateAsync(sub));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}