using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result) return NotFound();

        return Ok("User updated successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();

        return Ok("User deleted successfully");
    }

    [HttpPut("activate/{id}")]
    public async Task<IActionResult> Activate(int id)
    {
        var result = await _service.ActivateAsync(id);
        if (!result) return NotFound();

        return Ok("User activated");
    }

    [HttpPut("deactivate/{id}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var result = await _service.DeactivateAsync(id);
        if (!result) return NotFound();

        return Ok("User deactivated");
    }
}