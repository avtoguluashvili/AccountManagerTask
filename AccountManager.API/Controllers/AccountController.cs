using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IAccountService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Account>>> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetById(int id)
    {
        var acct = await service.GetByIdAsync(id);
        if (acct == null) return NotFound();
        return Ok(acct);
    }

    [HttpPost]
    public async Task<ActionResult<Account>> Create([FromBody] Account account, [FromQuery] int subscriptionId)
    {
        if (subscriptionId <= 0)
            return BadRequest("Invalid subscription ID.");

        try
        {
            var created = await service.CreateAsync(account, subscriptionId);
            return CreatedAtAction(nameof(GetById), new { id = created.AccountId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<Account>> Update(int id, [FromBody] Account account)
    {
        if (id != account.AccountId) return BadRequest();
        return Ok(await service.UpdateAsync(account));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Account>>> Search([FromQuery] string q)
    {
        return Ok(await service.SearchAsync(q));
    }
}