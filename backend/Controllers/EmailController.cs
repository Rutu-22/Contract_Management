using Assiginment.Model;
using Assiginment.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("SendApprovalEmail")]
    public async Task<IActionResult> SendApprovalEmail(int userId, string email)
    {
        try
        {
            await _emailService.SendApprovalEmailAsync(userId, email);
            return Ok("Email sent successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("UpdateStatus")]
    public async Task<IActionResult> UpdateStatus([FromBody] StatusUpdateRequest request)
    {
        try
        {
            await _emailService.UpdateStatusAsync(request);
            return Ok("Status updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
